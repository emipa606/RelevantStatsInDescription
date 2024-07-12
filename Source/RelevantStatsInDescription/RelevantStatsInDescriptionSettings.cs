using Verse;

namespace RelevantStatsInDescription;

/// <summary>
///     Definition of the relevantStatsInDescriptionSettings for the mod
/// </summary>
internal class RelevantStatsInDescriptionSettings : ModSettings
{
    public bool RelativeWork;
    public bool RemoveRotateWidget = true;
    public bool ShowAffordance = true;
    public bool ShowAffordanceRequirement = true;
    public bool ShowBeauty = true;
    public bool ShowBedRest = true;
    public bool ShowCleanliness = true;
    public bool ShowComfort = true;
    public bool ShowCover = true;
    public bool ShowDefName;
    public bool ShowDominantStyle = true;
    public bool ShowDPS;
    public bool ShowFloorQuality = true;
    public bool ShowHP = true;
    public bool ShowHPForAll;
    public bool ShowImmunityGainSpeed = true;
    public bool ShowJoy = true;
    public bool ShowJoyKind = true;
    public bool ShowMass = true;
    public bool ShowMedicalTendQuality = true;
    public bool ShowPowerConsumer = true;
    public bool ShowPowerProducer = true;
    public bool ShowResearchSpeed = true;
    public bool ShowRimefeller = true;
    public bool ShowSize = true;
    public bool ShowStorageSpace = true;
    public bool ShowSurgerySuccessChance = true;
    public bool ShowTechLevel;
    public bool ShowUIOrder;
    public bool ShowVFEGas = true;
    public bool ShowWealth = true;
    public bool ShowWorkToBuild = true;
    public bool UseTooltip;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowHP, "ShowHP", true);
        Scribe_Values.Look(ref ShowHPForAll, "ShowHPForAll");
        Scribe_Values.Look(ref ShowAffordance, "ShowAffordance", true);
        Scribe_Values.Look(ref ShowAffordanceRequirement, "ShowAffordanceRequirement", true);
        Scribe_Values.Look(ref ShowCover, "ShowCover", true);
        Scribe_Values.Look(ref ShowBeauty, "ShowBeauty", true);
        Scribe_Values.Look(ref ShowMass, "ShowMass", true);
        Scribe_Values.Look(ref ShowSize, "ShowSize", true);
        Scribe_Values.Look(ref ShowWealth, "ShowWealth", true);
        Scribe_Values.Look(ref ShowCleanliness, "ShowCleanliness", true);
        Scribe_Values.Look(ref ShowJoy, "ShowJoy", true);
        Scribe_Values.Look(ref ShowJoyKind, "ShowJoyKind", true);
        Scribe_Values.Look(ref ShowComfort, "ShowComfort", true);
        Scribe_Values.Look(ref ShowBedRest, "ShowBedRest", true);
        Scribe_Values.Look(ref ShowImmunityGainSpeed, "ShowImmunityGainSpeed", true);
        Scribe_Values.Look(ref ShowSurgerySuccessChance, "ShowSurgerySuccessChance", true);
        Scribe_Values.Look(ref ShowMedicalTendQuality, "ShowMedicalTendQuality", true);
        Scribe_Values.Look(ref ShowResearchSpeed, "ShowResearchSpeed", true);
        Scribe_Values.Look(ref ShowPowerConsumer, "ShowPowerConsumer", true);
        Scribe_Values.Look(ref ShowPowerProducer, "ShowPowerProducer", true);
        Scribe_Values.Look(ref ShowVFEGas, "ShowVFEGas", true);
        Scribe_Values.Look(ref ShowRimefeller, "ShowRimefeller", true);
        Scribe_Values.Look(ref ShowWorkToBuild, "ShowWorkToBuild", true);
        Scribe_Values.Look(ref RelativeWork, "RelativeWork");
        Scribe_Values.Look(ref ShowDPS, "ShowDPS");
        Scribe_Values.Look(ref UseTooltip, "UseTooltip");
        Scribe_Values.Look(ref ShowTechLevel, "ShowTechLevel");
        Scribe_Values.Look(ref ShowStorageSpace, "ShowStorageSpace", true);
        Scribe_Values.Look(ref ShowUIOrder, "ShowUIOrder");
        Scribe_Values.Look(ref ShowDefName, "ShowDefName");
        Scribe_Values.Look(ref RemoveRotateWidget, "RemoveRotateWidget", true);
        Scribe_Values.Look(ref ShowDominantStyle, "ShowDominantStyle", true);
        Scribe_Values.Look(ref ShowFloorQuality, "ShowFloorQuality", true);
    }
}