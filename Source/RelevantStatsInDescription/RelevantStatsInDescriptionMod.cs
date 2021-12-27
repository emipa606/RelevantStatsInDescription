using Mlie;
using UnityEngine;
using Verse;

namespace RelevantStatsInDescription;

[StaticConstructorOnStartup]
internal class RelevantStatsInDescriptionMod : Mod
{
    /// <summary>
    ///     The instance of the relevantStatsInDescriptionSettings to be read by the mod
    /// </summary>
    public static RelevantStatsInDescriptionMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private relevantStatsInDescriptionSettings
    /// </summary>
    private RelevantStatsInDescriptionSettings relevantStatsInDescriptionSettings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public RelevantStatsInDescriptionMod(ModContentPack content) : base(content)
    {
        instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.RelevantStatsInDescription"));
    }

    /// <summary>
    ///     The instance-relevantStatsInDescriptionSettings for the mod
    /// </summary>
    internal RelevantStatsInDescriptionSettings RelevantStatsInDescriptionSettings
    {
        get
        {
            if (relevantStatsInDescriptionSettings == null)
            {
                relevantStatsInDescriptionSettings = GetSettings<RelevantStatsInDescriptionSettings>();
            }

            return relevantStatsInDescriptionSettings;
        }
        set => relevantStatsInDescriptionSettings = value;
    }

    /// <summary>
    ///     The title for the mod-relevantStatsInDescriptionSettings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Relevant Stats In Description";
    }

    /// <summary>
    ///     The relevantStatsInDescriptionSettings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("RSID_ShowHP.Label".Translate(), ref RelevantStatsInDescriptionSettings.ShowHP,
            "RSID_ShowHP.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowAffordance.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowAffordance, "RSID_ShowAffordance.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowAffordanceRequirement.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowAffordanceRequirement,
            "RSID_ShowAffordanceRequirement.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowCover.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowCover,
            "RSID_ShowCover.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowBeauty.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowBeauty,
            "RSID_ShowBeauty.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowCleanliness.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowCleanliness,
            "RSID_ShowCleanliness.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowJoy.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowJoy,
            "RSID_ShowJoy.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowComfort.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowComfort,
            "RSID_ShowComfort.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowBedRest.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowBedRest,
            "RSID_ShowBedRest.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowImmunityGainSpeed.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowImmunityGainSpeed,
            "RSID_ShowImmunityGainSpeed.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowSurgerySuccessChance.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowSurgerySuccessChance,
            "RSID_ShowSurgerySuccessChance.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowMedicalTendQuality.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowMedicalTendQuality,
            "RSID_ShowMedicalTendQuality.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowPowerConsumer.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowPowerConsumer,
            "RSID_ShowPowerConsumer.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowPowerProducer.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowPowerProducer,
            "RSID_ShowPowerProducer.Tooltip".Translate());
        if (RelevantStatsInDescription.VFEPowerLoaded && (RelevantStatsInDescriptionSettings.ShowPowerConsumer ||
                                                          RelevantStatsInDescriptionSettings.ShowPowerProducer))
        {
            listing_Standard.CheckboxLabeled("RSID_ShowVFEGas.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowVFEGas,
                "RSID_ShowVFEGas.Tooltip".Translate());
        }

        if (RelevantStatsInDescription.RimefellerLoaded && (RelevantStatsInDescriptionSettings.ShowPowerConsumer ||
                                                            RelevantStatsInDescriptionSettings.ShowPowerProducer))
        {
            listing_Standard.CheckboxLabeled("RSID_ShowRimefeller.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowRimefeller,
                "RSID_ShowRimefeller.Tooltip".Translate());
        }

        listing_Standard.CheckboxLabeled("RSID_ShowWorkToBuild.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowWorkToBuild,
            "RSID_ShowWorkToBuild.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.ShowWorkToBuild)
        {
            listing_Standard.CheckboxLabeled("RSID_RelativeWork.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.RelativeWork,
                "RSID_RelativeWork.Tooltip".Translate());
        }
        else
        {
            RelevantStatsInDescriptionSettings.RelativeWork = false;
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("RSID_CurrentVersion.Label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        RelevantStatsInDescription.ClearCache();
    }
}