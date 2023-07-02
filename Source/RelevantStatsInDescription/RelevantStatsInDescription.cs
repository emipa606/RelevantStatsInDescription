using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RelevantStatsInDescription;

[StaticConstructorOnStartup]
public class RelevantStatsInDescription
{
    private static Dictionary<string, string> cachedDescriptions;

    public static readonly bool VFEPowerLoaded;
    public static readonly bool RimefellerLoaded;
    public static readonly bool LWMLoaded;

    static RelevantStatsInDescription()
    {
        VFEPowerLoaded = ModLister.GetActiveModWithIdentifier("VanillaExpanded.VFEPower") != null;
        RimefellerLoaded = ModLister.GetActiveModWithIdentifier("Dubwise.Rimefeller") != null;
        LWMLoaded = ModLister.GetActiveModWithIdentifier("LWM.DeepStorage") != null;
        cachedDescriptions = new Dictionary<string, string>();
        var harmony = new Harmony("Mlie.RelevantStatsInDescription");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    public static void ClearCache()
    {
        cachedDescriptions = new Dictionary<string, string>();
    }

    public static float GetExtraHeight()
    {
        return (typeof(RelevantStatsInDescriptionSettings).GetFields().Count(info =>
            (bool)info.GetValue(RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings)) * 5f) + 10f;
    }

    public static string GetUpdatedDescription(BuildableDef def, ThingDef stuff)
    {
        var descriptionKey = $"{def.defName}|{stuff?.defName}";
        if (cachedDescriptions.TryGetValue(descriptionKey, out var description))
        {
            return description + def.description;
        }

        var arrayToAdd = new List<string>();

        if (def is TerrainDef floorDef)
        {
            // Affordances
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowAffordance)
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
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowAffordanceRequirement)
            {
                var affordanceNeeded = floorDef.GetTerrainAffordanceNeed(stuff);
                if (affordanceNeeded != null &&
                    affordanceNeeded != TerrainAffordanceDefOf.Light)
                {
                    arrayToAdd.Add("RSID_AffordanceRequirement".Translate(affordanceNeeded.LabelCap));
                }
            }

            // Beauty
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowBeauty)
            {
                var beauty = floorDef.GetStatValueAbstract(StatDefOf.Beauty);
                if (beauty != 0)
                {
                    arrayToAdd.Add("RSID_Beauty".Translate(beauty));
                }
            }

            // Wealth
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowWealth)
            {
                var wealth = floorDef.GetStatValueAbstract(StatDefOf.MarketValue);
                if (wealth != 0)
                {
                    arrayToAdd.Add("RSID_Wealth".Translate(wealth.ToStringMoney()));
                }
            }

            // Cleanliness
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowCleanliness)
            {
                var cleanliness = floorDef.GetStatValueAbstract(StatDefOf.Cleanliness);
                if (cleanliness != 0)
                {
                    arrayToAdd.Add("RSID_Cleanliness".Translate(cleanliness.ToString("N1")));
                }
            }

            // TechLevel
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowTechLevel)
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
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowWorkToBuild)
            {
                var workToBuild = floorDef.GetStatValueAbstract(StatDefOf.WorkToBuild);
                if (workToBuild != 0)
                {
                    if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.RelativeWork)
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

            // Defname
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowDefName)
            {
                arrayToAdd.Add("RSID_DefName".Translate(floorDef.defName));
            }

            if (arrayToAdd.Any())
            {
                if (!string.IsNullOrEmpty(def.description))
                {
                    arrayToAdd.Add(" - - - \n");
                }

                cachedDescriptions[descriptionKey] = string.Join("\n", arrayToAdd);
            }
            else
            {
                cachedDescriptions[descriptionKey] = string.Empty;
            }

            return cachedDescriptions[descriptionKey] + floorDef.description;
        }

        if (def is not ThingDef buildableThing)
        {
            return def.description;
        }

        if (stuff == null && def.MadeFromStuff)
        {
            stuff = GenStuff.DefaultStuffFor(def);
        }

        var thing = new Thing { def = buildableThing };
        if (stuff != null)
        {
            thing.SetStuffDirect(stuff);
        }

        // Structural building
        if (buildableThing.graphicData?.linkFlags != null &&
            ((buildableThing.graphicData.linkFlags & LinkFlags.Wall) != 0 ||
             (buildableThing.graphicData.linkFlags & LinkFlags.Fences) != 0 ||
             (buildableThing.graphicData.linkFlags & LinkFlags.Barricades) != 0 ||
             (buildableThing.graphicData.linkFlags & LinkFlags.Sandbags) != 0) ||
            buildableThing.IsDoor)
        {
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowHP)
            {
                arrayToAdd.Add("RSID_MaxHP".Translate(thing.MaxHitPoints));
            }

            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowCover &&
                buildableThing.fillPercent < 1)
            {
                arrayToAdd.Add("RSID_Cover".Translate(buildableThing.fillPercent.ToStringPercent()));
            }
        }
        else
        {
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowHPForAll &&
                buildableThing.MadeFromStuff)
            {
                arrayToAdd.Add("RSID_MaxHP".Translate(thing.MaxHitPoints));
            }
        }

        // Comfort
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowComfort)
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
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowBedRest &&
                buildableThing.StatBaseDefined(StatDefOf.BedRestEffectiveness))
            {
                arrayToAdd.Add(
                    "RSID_BedRestEffectiveness".Translate(
                        thing.GetStatValue(StatDefOf.BedRestEffectiveness).ToStringPercent()));
            }

            if (buildableThing.building.bed_defaultMedical || !buildableThing.building.bed_humanlike)
            {
                if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowMedicalTendQuality &&
                    buildableThing.StatBaseDefined(StatDefOf.MedicalTendQualityOffset))
                {
                    arrayToAdd.Add("RSID_MedicalTendQuality".Translate(
                        thing.GetStatValue(StatDefOf.MedicalTendQualityOffset).ToStringPercent()));
                }

                if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowImmunityGainSpeed &&
                    buildableThing.StatBaseDefined(StatDefOf.ImmunityGainSpeedFactor))
                {
                    arrayToAdd.Add("RSID_ImmunityGainSpeedFactor".Translate(
                        thing.GetStatValue(StatDefOf.ImmunityGainSpeedFactor).ToStringPercent()));
                }

                if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings
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
            if (powerComp.compClass == typeof(CompPowerPlantSolar))
            {
                consumption = -CompPowerPlantSolar.FullSunPower;
            }

            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowPowerConsumer &&
                consumption > 0)
            {
                arrayToAdd.Add("RSID_PowerUser".Translate(consumption.ToString()));
            }

            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowPowerProducer &&
                consumption < 0)
            {
                arrayToAdd.Add("RSID_PowerProducer".Translate((consumption * -1).ToString()));
            }
        }

        // Chemfueluse
        if (RimefellerLoaded)
        {
            var chemfuelConsumtion = 0f;
            foreach (var compProperty in buildableThing.comps)
            {
                if (compProperty == null ||
                    compProperty.GetType().FullName?.EndsWith("Rimefeller.CompProperties_PowerPlant") == false)
                {
                    continue;
                }

                chemfuelConsumtion = (float)AccessTools.Field(AccessTools.TypeByName(compProperty.GetType().FullName),
                        "FuelRate")
                    .GetValue(compProperty);
            }

            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowRimefeller &&
                chemfuelConsumtion != 0)
            {
                arrayToAdd.Add("RSID_ChemfuelUser".Translate(chemfuelConsumtion.ToString()));
            }
        }

        // Gasuse
        if (VFEPowerLoaded && RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowVFEGas)
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
                if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowPowerConsumer &&
                    gasConsumption > 0)
                {
                    arrayToAdd.Add("RSID_GasUser".Translate(gasConsumption.ToString()));
                }

                if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowPowerProducer &&
                    gasConsumption < 0)
                {
                    arrayToAdd.Add("RSID_GasProducer".Translate((gasConsumption * -1).ToString()));
                }
            }
        }

        // Beauty
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowBeauty)
        {
            var beauty = thing.GetStatValue(StatDefOf.Beauty);
            if (beauty != 0)
            {
                arrayToAdd.Add("RSID_Beauty".Translate(beauty));
            }
        }

        // Mass
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowMass &&
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
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowSize)
        {
            arrayToAdd.Add("RSID_Size".Translate(def.Size.ToStringCross()));
        }

        // Wealth
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowWealth)
        {
            var wealth = thing.GetStatValue(StatDefOf.MarketValueIgnoreHp);
            if (wealth != 0)
            {
                arrayToAdd.Add("RSID_Wealth".Translate(wealth.ToStringMoney()));
            }
        }

        // Cleanliness
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowCleanliness)
        {
            var cleanliness = thing.GetStatValue(StatDefOf.Cleanliness);
            if (cleanliness != 0)
            {
                arrayToAdd.Add("RSID_Cleanliness".Translate(cleanliness.ToString("N1")));
            }
        }

        // TechLevel
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowTechLevel)
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
            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowJoy)
            {
                var joy = thing.GetStatValue(StatDefOf.JoyGainFactor);
                if (joy != 0)
                {
                    arrayToAdd.Add("RSID_Joy".Translate(joy.ToStringPercent()));
                }
            }

            if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowJoyKind)
            {
                var joyKind = buildableThing.building?.joyKind;
                if (joyKind != null)
                {
                    arrayToAdd.Add("RSID_JoyKind".Translate(joyKind.LabelCap));
                }
            }
        }

        // Storage
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowStorageSpace)
        {
            if (buildableThing.building.fixedStorageSettings != null)
            {
                var maxItems = buildableThing.building.maxItemsInCell;
                var cells = buildableThing.size.x * buildableThing.size.z;
                arrayToAdd.Add("RSID_StorageSpace".Translate(maxItems * cells));
            }
        }

        // Affordance requirement
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowAffordanceRequirement)
        {
            var affordanceNeeded = buildableThing.GetTerrainAffordanceNeed(stuff);
            if (affordanceNeeded != null &&
                affordanceNeeded != TerrainAffordanceDefOf.Light)
            {
                arrayToAdd.Add("RSID_AffordanceRequirement".Translate(affordanceNeeded.LabelCap));
            }
        }

        // Work to build
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowWorkToBuild)
        {
            var workToBuild = buildableThing.GetStatValueAbstract(StatDefOf.WorkToBuild, stuff);
            if (workToBuild != 0)
            {
                if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.RelativeWork)
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

        // Defname
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.ShowDefName)
        {
            arrayToAdd.Add("RSID_DefName".Translate(buildableThing.defName));
        }

        if (arrayToAdd.Any())
        {
            arrayToAdd.Add(" - - - \n");
            cachedDescriptions[descriptionKey] = string.Join("\n", arrayToAdd);
        }
        else
        {
            cachedDescriptions[descriptionKey] = string.Empty;
        }

        return cachedDescriptions[descriptionKey] + buildableThing.description;
    }
}