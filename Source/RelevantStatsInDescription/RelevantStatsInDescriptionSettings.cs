using Verse;

namespace RelevantStatsInDescription;

/// <summary>
///     Definition of the relevantStatsInDescriptionSettings for the mod
/// </summary>
internal class RelevantStatsInDescriptionSettings : ModSettings
{
    public bool RelativeWork;
    public bool ShowAffordance = true;
    public bool ShowAffordanceRequirement = true;
    public bool ShowBeauty = true;
    public bool ShowBedRest = true;
    public bool ShowCleanliness = true;
    public bool ShowComfort = true;
    public bool ShowCover = true;
    public bool ShowHP = true;
    public bool ShowImmunityGainSpeed = true;
    public bool ShowJoy = true;
    public bool ShowMedicalTendQuality = true;
    public bool ShowPowerConsumer = true;
    public bool ShowPowerProducer = true;
    public bool ShowRimefeller = true;
    public bool ShowSurgerySuccessChance = true;
    public bool ShowVFEGas = true;
    public bool ShowWorkToBuild = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowHP, "ShowHP", true);
        Scribe_Values.Look(ref ShowAffordance, "ShowAffordance", true);
        Scribe_Values.Look(ref ShowAffordanceRequirement, "ShowAffordanceRequirement", true);
        Scribe_Values.Look(ref ShowCover, "ShowCover", true);
        Scribe_Values.Look(ref ShowBeauty, "ShowBeauty", true);
        Scribe_Values.Look(ref ShowCleanliness, "ShowCleanliness", true);
        Scribe_Values.Look(ref ShowJoy, "ShowJoy", true);
        Scribe_Values.Look(ref ShowComfort, "ShowComfort", true);
        Scribe_Values.Look(ref ShowBedRest, "ShowBedRest", true);
        Scribe_Values.Look(ref ShowImmunityGainSpeed, "ShowImmunityGainSpeed", true);
        Scribe_Values.Look(ref ShowSurgerySuccessChance, "ShowSurgerySuccessChance", true);
        Scribe_Values.Look(ref ShowMedicalTendQuality, "ShowMedicalTendQuality", true);
        Scribe_Values.Look(ref ShowPowerConsumer, "ShowPowerConsumer", true);
        Scribe_Values.Look(ref ShowPowerProducer, "ShowPowerProducer", true);
        Scribe_Values.Look(ref ShowVFEGas, "ShowVFEGas", true);
        Scribe_Values.Look(ref ShowRimefeller, "ShowRimefeller", true);
        Scribe_Values.Look(ref ShowWorkToBuild, "ShowWorkToBuild", true);
        Scribe_Values.Look(ref RelativeWork, "RelativeWork");
    }
}