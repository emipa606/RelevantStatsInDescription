using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RelevantStatsInDescription;

[StaticConstructorOnStartup]
public class RelevantStatsInDescription
{
    private static Dictionary<string, string> cachedDescriptions;

    public static readonly bool VfePowerLoaded;
    public static readonly bool RimefellerLoaded;
    public static readonly bool RepowerOnOffLoaded;
    public static readonly bool LightsOutLoaded;
    private static readonly Dictionary<ThingDef, float> turretDps;
    private static readonly FieldInfo repowerOnOffPowerLevels;
    private static readonly MethodInfo lightsOutPostfix;
    private static readonly PropertyInfo lightsOutActiveResourceDrawRate;
    private static readonly FieldInfo verbsFieldInfo = AccessTools.Field(typeof(ThingDef), "verbs");

    private static readonly FieldInfo damageAmountBaseFieldInfo =
        AccessTools.Field(typeof(ProjectileProperties), "damageAmountBase");

    static RelevantStatsInDescription()
    {
        VfePowerLoaded = ModLister.GetActiveModWithIdentifier("VanillaExpanded.VFEPower", true) != null;
        RimefellerLoaded = ModLister.GetActiveModWithIdentifier("Dubwise.Rimefeller", true) != null;
        RepowerOnOffLoaded = ModLister.GetActiveModWithIdentifier("Mlie.TurnOnOffRePowered", true) != null;
        LightsOutLoaded = ModLister.GetActiveModWithIdentifier("juanlopez2008.LightsOut", true) != null;
        cachedDescriptions = new Dictionary<string, string>();
        var harmony = new Harmony("Mlie.RelevantStatsInDescription");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        turretDps = [];
        foreach (var turret in DefDatabase<ThingDef>.AllDefs.Where(def => def.building?.turretGunDef != null))
        {
            var verbs = (List<VerbProperties>)verbsFieldInfo.GetValue(turret.building.turretGunDef);
            if (verbs == null || !verbs.Any())
            {
                Log.Message(
                    $"[RelevantStatsInDescription]: Skipping dps-calculation of {turret.LabelCap} as it has no valid attack-verbs");
                continue;
            }

            var attackVerb = verbs[0];
            var bullet = attackVerb.defaultProjectile;
            if (bullet?.projectile == null)
            {
                Log.Message(
                    $"[RelevantStatsInDescription]: Skipping dps-calculation of {turret.LabelCap} as it has no default projectile defined");
                continue;
            }

            if (!bullet.projectile.damageDef.harmsHealth)
            {
                Log.Message(
                    $"[RelevantStatsInDescription]: Skipping dps-calculation of {turret.LabelCap} as its projectile does not cause damage to health");
                continue;
            }

            var cooldown = 0f;
            if (turret.building.turretGunDef.StatBaseDefined(StatDefOf.RangedWeapon_Cooldown))
            {
                cooldown = turret.building.turretGunDef.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown);
            }

            var damageAmountBase = (int)damageAmountBaseFieldInfo.GetValue(bullet.projectile);
            float burstDamage = damageAmountBase * attackVerb.burstShotCount;
            var warmupTicks = (cooldown + attackVerb.warmupTime) * 60;
            float burstTicks = (attackVerb.burstShotCount - 1) * attackVerb.ticksBetweenBurstShots;
            var totalTime = (warmupTicks + burstTicks) / 60;

            turretDps[turret] = (float)Math.Round(burstDamage / totalTime, 2);
        }

        if (RepowerOnOffLoaded)
        {
            repowerOnOffPowerLevels = AccessTools.Field("TurnOnOffRePowered.TurnItOnUtility:powerLevels");
        }

        if (!LightsOutLoaded)
        {
            return;
        }

        lightsOutPostfix = AccessTools.Method("LightsOut.Patches.Power.DisableBasePowerDrawOnGet:Postfix");
        lightsOutActiveResourceDrawRate =
            AccessTools.Property("LightsOut.Boilerplate.ModSettings:ActiveResourceDrawRate");
    }

    public static void ClearCache()
    {
        cachedDescriptions = new Dictionary<string, string>();
        Log.Message("[RelevantStatsInDescription]: Clearing cached descriptions");
    }

    public static float GetExtraHeight()
    {
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.UseTooltip)
        {
            return 0f;
        }

        return (typeof(RelevantStatsInDescriptionSettings).GetFields().Count(info =>
            (bool)info.GetValue(RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings)) * 5f) + 10f;
    }

    public static string GetUpdatedDescription(BuildableDef def, ThingDef stuff, bool onlyAddition = false)
    {
        var descriptionKey = $"{def.defName}|{stuff?.defName}";
        if (cachedDescriptions.TryGetValue(descriptionKey, out var description))
        {
            if (onlyAddition)
            {
                return description;
            }

            return description + def.description;
        }

        var arrayToAdd = new List<string>();

        if (def is TerrainDef floorDef)
        {
            // Affordances
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowAffordance)
            {
                if (floorDef.affordances?.Any() == true && !floorDef.affordances.Contains(TerrainAffordanceDefOf.Heavy))
                {
                    if (floorDef.affordances.Contains(TerrainAffordanceDefOf.Medium))
                    {
                        arrayToAdd.Add("RSID_MaxAffordance".Translate(TerrainAffordanceDefOf.Medium.LabelCap));
                    }
                    else if (floorDef.affordances.Contains(TerrainAffordanceDefOf.Light))
                    {
                        arrayToAdd.Add("RSID_MaxAffordance".Translate(TerrainAffordanceDefOf.Light.LabelCap));
                    }
                    else
                    {
                        arrayToAdd.Add("RSID_MaxAffordance".Translate("RSID_Undefined".Translate()));
                    }
                }
            }

            // Affordance requirement
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowAffordanceRequirement)
            {
                var affordanceNeeded = floorDef.GetTerrainAffordanceNeed(stuff);
                if (affordanceNeeded != null &&
                    affordanceNeeded != TerrainAffordanceDefOf.Light)
                {
                    arrayToAdd.Add("RSID_AffordanceRequirement".Translate(affordanceNeeded.LabelCap));
                }
            }


            // Dominant style
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowDominantStyle &&
                ModsConfig.IdeologyActive)
            {
                var styleCategory = floorDef.dominantStyleCategory;

                if (styleCategory != null)
                {
                    arrayToAdd.Add("RSID_DominantStyle".Translate(styleCategory.LabelCap));
                }
            }

            // Beauty
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowBeauty)
            {
                var beauty = floorDef.GetStatValueAbstract(StatDefOf.Beauty);
                if (beauty != 0)
                {
                    arrayToAdd.Add("RSID_Beauty".Translate(beauty));
                }
            }

            // Wealth
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowWealth)
            {
                var wealth = floorDef.GetStatValueAbstract(StatDefOf.MarketValue);
                if (wealth != 0)
                {
                    arrayToAdd.Add("RSID_Wealth".Translate(wealth.ToStringMoney()));
                }
            }

            // Cleanliness
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowCleanliness)
            {
                var cleanliness = floorDef.GetStatValueAbstract(StatDefOf.Cleanliness);
                if (cleanliness != 0)
                {
                    arrayToAdd.Add("RSID_Cleanliness".Translate(cleanliness.ToString("N1")));
                }
            }

            // TechLevel
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowTechLevel)
            {
                if (floorDef.researchPrerequisites?.Any() == true)
                {
                    var techLevel = (int)TechLevel.Undefined;
                    foreach (var researchProjectDef in floorDef.researchPrerequisites)
                    {
                        if ((int)researchProjectDef.techLevel > techLevel)
                        {
                            techLevel = (int)researchProjectDef.techLevel;
                        }
                    }

                    if (techLevel > 0)
                    {
                        arrayToAdd.Add("RSID_TechLevel".Translate(((TechLevel)techLevel).ToString()));
                    }
                }
            }

            // Work to build
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowWorkToBuild)
            {
                var workToBuild = floorDef.GetStatValueAbstract(StatDefOf.WorkToBuild);
                if (workToBuild != 0)
                {
                    if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.RelativeWork)
                    {
                        switch (workToBuild)
                        {
                            case < 1000:
                                arrayToAdd.Add("RSID_WorkRelative".Translate("RSID_Small".Translate()));
                                break;
                            case < 5000:
                                arrayToAdd.Add("RSID_WorkRelative".Translate("RSID_Medium".Translate()));
                                break;
                            default:
                                arrayToAdd.Add("RSID_WorkRelative".Translate("RSID_Large".Translate()));
                                break;
                        }
                    }
                    else
                    {
                        arrayToAdd.Add("RSID_WorkExact".Translate(Math.Ceiling(workToBuild / 60)));
                    }
                }
            }

            // Floor Quality (Royalty)
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowFloorQuality &&
                ModsConfig.RoyaltyActive)
            {
                arrayToAdd.Add("RSID_FloorQuality".Translate(floorDef.IsFine
                    ? "RSID_FloorQuality_Fine".Translate()
                    : "RSID_FloorQuality_Common".Translate()));
            }

            // Defname
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowDefName)
            {
                arrayToAdd.Add("RSID_DefName".Translate(floorDef.defName));
            }

            if (arrayToAdd.Any())
            {
                if (!string.IsNullOrEmpty(def.description) && !onlyAddition)
                {
                    arrayToAdd.Add(" - - - \n");
                }

                cachedDescriptions[descriptionKey] = string.Join("\n", arrayToAdd);
            }
            else
            {
                cachedDescriptions[descriptionKey] = string.Empty;
            }

            if (onlyAddition)
            {
                return cachedDescriptions[descriptionKey];
            }

            return cachedDescriptions[descriptionKey] + floorDef.description;
        }

        if (def is not ThingDef buildableThing)
        {
            return onlyAddition ? string.Empty : def.description;
        }

        if (stuff == null && def.MadeFromStuff)
        {
            stuff = GenStuff.DefaultStuffFor(def);
        }

        var thing = new ThingWithComps { def = buildableThing };
        if (stuff != null)
        {
            thing.SetStuffDirect(stuff);
        }

        if (buildableThing.comps.Any())
        {
            if (buildableThing.thingClass.Name != "Building_Window")
            {
                thing.InitializeComps();
                thing.PostMake();
                thing.PostPostMake();
            }
        }

        // Structural building
        if (buildableThing.graphicData?.linkFlags != null &&
            ((buildableThing.graphicData.linkFlags & LinkFlags.Wall) != 0 ||
             (buildableThing.graphicData.linkFlags & LinkFlags.Fences) != 0 ||
             (buildableThing.graphicData.linkFlags & LinkFlags.Barricades) != 0 ||
             (buildableThing.graphicData.linkFlags & LinkFlags.Sandbags) != 0) ||
            buildableThing.IsDoor)
        {
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowHP)
            {
                arrayToAdd.Add("RSID_MaxHP".Translate(thing.MaxHitPoints));
            }

            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowCover &&
                buildableThing.fillPercent < 1)
            {
                arrayToAdd.Add("RSID_Cover".Translate(buildableThing.fillPercent.ToStringPercent()));
            }
        }
        else
        {
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowHPForAll &&
                buildableThing.MadeFromStuff)
            {
                arrayToAdd.Add("RSID_MaxHP".Translate(thing.MaxHitPoints));
            }
        }

        // DPS
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowDPS &&
            turretDps.TryGetValue(buildableThing, out var dps))
        {
            arrayToAdd.Add("RSID_DPS".Translate(dps));
        }

        // Comfort
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowComfort)
        {
            var comfort = thing.GetStatValue(StatDefOf.Comfort);

            if (comfort > 0)
            {
                arrayToAdd.Add(
                    "RSID_Comfort".Translate(comfort.ToStringPercent()));
            }
        }

        // Bed
        if (buildableThing.IsBed)
        {
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowBedRest &&
                buildableThing.StatBaseDefined(StatDefOf.BedRestEffectiveness))
            {
                arrayToAdd.Add(
                    "RSID_BedRestEffectiveness".Translate(
                        thing.GetStatValue(StatDefOf.BedRestEffectiveness).ToStringPercent()));
            }

            if (buildableThing.building.bed_defaultMedical || !buildableThing.building.bed_humanlike)
            {
                if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowMedicalTendQuality &&
                    buildableThing.StatBaseDefined(StatDefOf.MedicalTendQualityOffset))
                {
                    arrayToAdd.Add("RSID_MedicalTendQuality".Translate(
                        thing.GetStatValue(StatDefOf.MedicalTendQualityOffset).ToStringPercent()));
                }

                if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowImmunityGainSpeed &&
                    buildableThing.StatBaseDefined(StatDefOf.ImmunityGainSpeedFactor))
                {
                    arrayToAdd.Add("RSID_ImmunityGainSpeedFactor".Translate(
                        thing.GetStatValue(StatDefOf.ImmunityGainSpeedFactor).ToStringPercent()));
                }

                if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings
                        .ShowSurgerySuccessChance &&
                    buildableThing.StatBaseDefined(StatDefOf.SurgerySuccessChanceFactor))
                {
                    arrayToAdd.Add("RSID_SurgerySuccessChanceFactor".Translate(
                        thing.GetStatValue(StatDefOf.SurgerySuccessChanceFactor)
                            .ToStringPercent()));
                }
            }
        }

        // Poweruse
        var powerComp = buildableThing.GetCompProperties<CompProperties_Power>();
        if (powerComp != null)
        {
            var consumption = powerComp.PowerConsumption;

            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowPowerProducer &&
                consumption < 0)
            {
                arrayToAdd.Add("RSID_PowerProducer".Translate((consumption * -1).ToString()));
            }

            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowPowerConsumer &&
                consumption > 0)
            {
                var variedConsumption = getMinMaxPower(buildableThing, thing, -consumption);
                arrayToAdd.Add(variedConsumption == null
                    ? "RSID_PowerUser".Translate(consumption.ToString())
                    : "RSID_PowerUserVaried".Translate(variedConsumption.Item2, variedConsumption.Item1));
            }
        }

        // Chemfueluse
        if (RimefellerLoaded)
        {
            var chemfuelConsumption = 0f;
            foreach (var compProperty in buildableThing.comps)
            {
                if (compProperty == null ||
                    compProperty.GetType().FullName?.EndsWith("Rimefeller.CompProperties_PowerPlant") == false)
                {
                    continue;
                }

                chemfuelConsumption = (float)AccessTools.Field(AccessTools.TypeByName(compProperty.GetType().FullName),
                        "FuelRate")
                    .GetValue(compProperty);
            }

            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowRimefeller &&
                chemfuelConsumption != 0)
            {
                arrayToAdd.Add("RSID_ChemfuelUser".Translate(chemfuelConsumption.ToString()));
            }
        }

        // Gasuse
        if (VfePowerLoaded && RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowVFEGas)
        {
            var gasConsumption = 0f;
            foreach (var compProperty in buildableThing.comps)
            {
                if (compProperty == null ||
                    compProperty.GetType().FullName?.EndsWith("GasNetwork.CompProperties_GasTrader") == false)
                {
                    continue;
                }

                gasConsumption = (float)AccessTools.Field(AccessTools.TypeByName(compProperty.GetType().FullName),
                        "gasConsumption")
                    .GetValue(compProperty);
            }

            if (gasConsumption != 0)
            {
                if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowPowerConsumer &&
                    gasConsumption > 0)
                {
                    arrayToAdd.Add("RSID_GasUser".Translate(gasConsumption.ToString()));
                }

                if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowPowerProducer &&
                    gasConsumption < 0)
                {
                    arrayToAdd.Add("RSID_GasProducer".Translate((gasConsumption * -1).ToString()));
                }
            }
        }

        // Beauty
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowBeauty)
        {
            var beauty = thing.GetStatValue(StatDefOf.Beauty);
            if (beauty != 0)
            {
                arrayToAdd.Add("RSID_Beauty".Translate(beauty));
            }
        }

        // Mass
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowMass &&
            buildableThing.Minifiable)
        {
            var mass = buildableThing.comps?.Any(properties => properties.compClass.Name == "CompWaterStorage") == true
                ? buildableThing.GetStatValueAbstract(StatDefOf.Mass)
                : thing.GetStatValue(StatDefOf.Mass);

            if (mass != 0)
            {
                arrayToAdd.Add("RSID_Mass".Translate(mass.ToStringMass()));
            }
        }

        // Size
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowSize)
        {
            arrayToAdd.Add("RSID_Size".Translate(def.Size.ToStringCross()));
        }

        // Wealth
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowWealth)
        {
            var wealth = thing.GetStatValue(StatDefOf.MarketValueIgnoreHp);
            if (wealth != 0)
            {
                arrayToAdd.Add("RSID_Wealth".Translate(wealth.ToStringMoney()));
            }
        }

        // Research Speed
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowResearchSpeed &&
            buildableThing.thingClass == typeof(Building_ResearchBench) &&
            buildableThing.StatBaseDefined(StatDefOf.ResearchSpeedFactor))
        {
            arrayToAdd.Add(
                "RSID_ResearchSpeedFactor".Translate(
                    thing.GetStatValue(StatDefOf.ResearchSpeedFactor).ToStringPercent()));
        }

        // Cleanliness
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowCleanliness)
        {
            var cleanliness = thing.GetStatValue(StatDefOf.Cleanliness);
            if (cleanliness != 0)
            {
                arrayToAdd.Add("RSID_Cleanliness".Translate(cleanliness.ToString("N1")));
            }
        }

        // TechLevel
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowTechLevel)
        {
            if (buildableThing.researchPrerequisites?.Any() == true)
            {
                var techLevel = (int)TechLevel.Undefined;
                foreach (var researchProjectDef in buildableThing.researchPrerequisites)
                {
                    if ((int)researchProjectDef.techLevel > techLevel)
                    {
                        techLevel = (int)researchProjectDef.techLevel;
                    }
                }

                if (techLevel > 0)
                {
                    arrayToAdd.Add("RSID_TechLevel".Translate(((TechLevel)techLevel).ToString()));
                }
            }
        }

        // Joy
        if (buildableThing.StatBaseDefined(StatDefOf.JoyGainFactor))
        {
            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowJoy)
            {
                var joy = thing.GetStatValue(StatDefOf.JoyGainFactor);
                if (joy != 0)
                {
                    arrayToAdd.Add("RSID_Joy".Translate(joy.ToStringPercent()));
                }
            }

            if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowJoyKind)
            {
                var joyKind = buildableThing.building?.joyKind;
                if (joyKind != null)
                {
                    arrayToAdd.Add("RSID_JoyKind".Translate(joyKind.LabelCap));
                }
            }
        }

        // Storage
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowStorageSpace)
        {
            if (buildableThing.building.fixedStorageSettings != null ||
                buildableThing.building.defaultStorageSettings != null)
            {
                var maxItems = buildableThing.building.maxItemsInCell;
                var cells = buildableThing.size.x * buildableThing.size.z;
                arrayToAdd.Add("RSID_StorageSpace".Translate(maxItems * cells));
            }
        }

        // Affordance requirement
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowAffordanceRequirement)
        {
            var affordanceNeeded = buildableThing.GetTerrainAffordanceNeed(stuff);
            if (affordanceNeeded != null &&
                affordanceNeeded != TerrainAffordanceDefOf.Light)
            {
                arrayToAdd.Add("RSID_AffordanceRequirement".Translate(affordanceNeeded.LabelCap));
            }
        }

        // Door Open Speed
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowDoorOpenSpeed &&
            buildableThing.IsDoor)
        {
            var doorOpenSpeed = thing.GetStatValue(StatDefOf.DoorOpenSpeed);
            if (doorOpenSpeed > 0)
            {
                arrayToAdd.Add("RSID_DoorOpenSpeed".Translate(doorOpenSpeed.ToStringPercent()));
            }
        }

        // Dominant style
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowDominantStyle &&
            ModsConfig.IdeologyActive)
        {
            var styleCategory = buildableThing.dominantStyleCategory;
            if (styleCategory == null && Faction.OfPlayer.ideos?.PrimaryIdeo != null)
            {
                styleCategory = Faction.OfPlayer.ideos.PrimaryIdeo.GetStyleCategoryFor(buildableThing);
            }

            if (styleCategory != null)
            {
                arrayToAdd.Add("RSID_DominantStyle".Translate(styleCategory.LabelCap));
            }
        }

        // Work to build
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowWorkToBuild)
        {
            var workToBuild = buildableThing.GetStatValueAbstract(StatDefOf.WorkToBuild, stuff);
            if (workToBuild != 0)
            {
                if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.RelativeWork)
                {
                    switch (workToBuild)
                    {
                        case < 1000:
                            arrayToAdd.Add("RSID_WorkRelative".Translate("RSID_Small".Translate()));
                            break;
                        case < 5000:
                            arrayToAdd.Add("RSID_WorkRelative".Translate("RSID_Medium".Translate()));
                            break;
                        default:
                            arrayToAdd.Add("RSID_WorkRelative".Translate("RSID_Large".Translate()));
                            break;
                    }
                }
                else
                {
                    arrayToAdd.Add(
                        "RSID_WorkExact".Translate(Math.Ceiling(workToBuild / 60)));
                }
            }
        }

        // UI Order
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowUIOrder)
        {
            arrayToAdd.Add("RSID_UIOrder".Translate(buildableThing.uiOrder));
        }

        // Defname
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowDefName)
        {
            arrayToAdd.Add("RSID_DefName".Translate(buildableThing.defName));
        }

        if (arrayToAdd.Any())
        {
            if (!onlyAddition)
            {
                arrayToAdd.Add(" - - - \n");
            }

            cachedDescriptions[descriptionKey] = string.Join("\n", arrayToAdd);
        }
        else
        {
            cachedDescriptions[descriptionKey] = string.Empty;
        }

        if (onlyAddition)
        {
            return cachedDescriptions[descriptionKey];
        }

        return cachedDescriptions[descriptionKey] + buildableThing.description;
    }

    private static Tuple<float, float> getMinMaxPower(ThingDef buildableThing, Thing thing, float originalConsumption)
    {
        if (RepowerOnOffLoaded)
        {
            var powerValues = (Dictionary<string, Vector2>)repowerOnOffPowerLevels.GetValue(null);
            if (powerValues == null || !powerValues.Any())
            {
                return null;
            }

            if (!powerValues.ContainsKey(buildableThing.defName))
            {
                return null;
            }

            return new Tuple<float, float>(powerValues[buildableThing.defName][0] * -1,
                powerValues[buildableThing.defName][1] * -1);
        }

        if (!LightsOutLoaded)
        {
            return null;
        }

        var itemCompPowerTrader = thing.TryGetComp<CompPowerTrader>();
        if (itemCompPowerTrader == null)
        {
            return null;
        }


        var arguments = new object[] { itemCompPowerTrader, originalConsumption, false };
        lightsOutPostfix.Invoke(null, arguments);
        var lowValue = (float)arguments[1];


        var highPowerFactor = (float)lightsOutActiveResourceDrawRate.GetValue(null);

        var highValue = originalConsumption;
        if (buildableThing.hasInteractionCell)
        {
            highValue *= highPowerFactor;
        }

        return new Tuple<float, float>(lowValue * -1, highValue * -1);
    }


    public static void ShowTooltip(Rect rect, string toolTip)
    {
        Text.Font = GameFont.Small;

        var height = Text.CalcHeight(toolTip, rect.width - 20f) + 24f;
        height = (float)Math.Ceiling(height / 50f) * 50f;

        var tooltipRect = new Rect(rect.x + rect.width + 5, rect.y, rect.width, height);

        while (Mouse.IsOver(tooltipRect.ExpandedBy(10f)))
        {
            tooltipRect.x = Event.current.mousePosition.x + 20f;
        }

        Find.WindowStack.ImmediateWindow(1266324534, tooltipRect, WindowLayer.Super, delegate
        {
            Text.Font = GameFont.Small;
            var atZero = tooltipRect.AtZero();
            Widgets.DrawWindowBackground(atZero);
            var rect2 = atZero.ContractedBy(10f);
            Widgets.BeginGroup(rect2);
            Widgets.Label(new Rect(0f, 0f, rect2.width, rect2.height), toolTip);
            Widgets.EndGroup();
        }, false);
    }
}