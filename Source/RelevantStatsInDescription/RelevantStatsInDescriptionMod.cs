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
    public static RelevantStatsInDescriptionMod Instance;

    private static string currentVersion;

    //private Vector2 scrollPosition;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public RelevantStatsInDescriptionMod(ModContentPack content) : base(content)
    {
        Instance = this;
        RelevantStatsInDescriptionSettings = GetSettings<RelevantStatsInDescriptionSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-relevantStatsInDescriptionSettings for the mod
    /// </summary>
    internal RelevantStatsInDescriptionSettings RelevantStatsInDescriptionSettings { get; }

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
        //var viewRect = new Rect(rect)
        //{
        //    width = rect.width * 0.97f,
        //    height = (typeof(RelevantStatsInDescriptionSettings).GetFields().Length * 25f) + 10f
        //};
        //Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.ColumnWidth = (rect.width / 2) - 10f;
        listingStandard.CheckboxLabeled("RSID_ShowHP.Label".Translate(), ref RelevantStatsInDescriptionSettings.ShowHP,
            "RSID_ShowHP.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.ShowHP)
        {
            listingStandard.CheckboxLabeled("RSID_ShowHPForAll.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowHPForAll,
                "RSID_ShowHPForAll.Tooltip".Translate());
        }
        else
        {
            RelevantStatsInDescriptionSettings.ShowHPForAll = false;
        }

        listingStandard.CheckboxLabeled("RSID_ShowAffordance.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowAffordance, "RSID_ShowAffordance.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowAffordanceRequirement.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowAffordanceRequirement,
            "RSID_ShowAffordanceRequirement.Tooltip".Translate());
        if (ModsConfig.IdeologyActive)
        {
            listingStandard.CheckboxLabeled("RSID_ShowDominantStyle.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowDominantStyle,
                "RSID_ShowDominantStyle.Tooltip".Translate());
        }

        listingStandard.CheckboxLabeled("RSID_ShowCover.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowCover,
            "RSID_ShowCover.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowBeauty.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowBeauty,
            "RSID_ShowBeauty.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowWealth.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowWealth,
            "RSID_ShowWealth.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowMass.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowMass,
            "RSID_ShowMass.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowDPS.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowDPS,
            "RSID_ShowDPS.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowSize.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowSize,
            "RSID_ShowSize.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowCleanliness.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowCleanliness,
            "RSID_ShowCleanliness.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowTechLevel.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowTechLevel,
            "RSID_ShowTechLevel.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowJoy.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowJoy,
            "RSID_ShowJoy.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowJoyKind.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowJoyKind,
            "RSID_ShowJoyKind.Tooltip".Translate());
        listingStandard.NewColumn();
        listingStandard.CheckboxLabeled("RSID_ShowComfort.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowComfort,
            "RSID_ShowComfort.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowBedRest.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowBedRest,
            "RSID_ShowBedRest.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowImmunityGainSpeed.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowImmunityGainSpeed,
            "RSID_ShowImmunityGainSpeed.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowSurgerySuccessChance.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowSurgerySuccessChance,
            "RSID_ShowSurgerySuccessChance.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowMedicalTendQuality.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowMedicalTendQuality,
            "RSID_ShowMedicalTendQuality.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowResearchSpeed.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowResearchSpeed,
            "RSID_ShowResearchSpeed.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowPowerConsumer.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowPowerConsumer,
            "RSID_ShowPowerConsumer.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.ShowPowerConsumer && (RelevantStatsInDescription.RepowerOnOffLoaded ||
                                                                     RelevantStatsInDescription.LightsOutLoaded))
        {
            listingStandard.Label("RSID_VariedPowerModLoaded.Label".Translate(), -1f,
                "RSID_VariedPowerModLoaded.Tooltip".Translate());
        }

        listingStandard.CheckboxLabeled("RSID_ShowPowerProducer.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowPowerProducer,
            "RSID_ShowPowerProducer.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowStorageSpace.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowStorageSpace,
            "RSID_ShowStorageSpace.Tooltip".Translate());
        if (RelevantStatsInDescription.VfePowerLoaded && (RelevantStatsInDescriptionSettings.ShowPowerConsumer ||
                                                          RelevantStatsInDescriptionSettings.ShowPowerProducer))
        {
            listingStandard.CheckboxLabeled("RSID_ShowVFEGas.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowVFEGas,
                "RSID_ShowVFEGas.Tooltip".Translate());
        }

        if (RelevantStatsInDescription.RimefellerLoaded && (RelevantStatsInDescriptionSettings.ShowPowerConsumer ||
                                                            RelevantStatsInDescriptionSettings.ShowPowerProducer))
        {
            listingStandard.CheckboxLabeled("RSID_ShowRimefeller.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowRimefeller,
                "RSID_ShowRimefeller.Tooltip".Translate());
        }

        listingStandard.CheckboxLabeled("RSID_ShowWorkToBuild.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowWorkToBuild,
            "RSID_ShowWorkToBuild.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.ShowWorkToBuild)
        {
            listingStandard.CheckboxLabeled("RSID_RelativeWork.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.RelativeWork,
                "RSID_RelativeWork.Tooltip".Translate());
        }
        else
        {
            RelevantStatsInDescriptionSettings.RelativeWork = false;
        }

        listingStandard.CheckboxLabeled("RSID_ShowFloorQuality.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowFloorQuality,
            "RSID_ShowFloorQuality.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowDoorOpenSpeed.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowDoorOpenSpeed,
            "RSID_ShowDoorOpenSpeed.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowDefName.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowDefName,
            "RSID_ShowDefName.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_ShowUIOrder.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowUIOrder,
            "RSID_ShowUIOrder.Tooltip".Translate());
        listingStandard.CheckboxLabeled("RSID_RemoveRotateWidget.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.RemoveRotateWidget,
            "RSID_RemoveRotateWidget.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.RemoveRotateWidget)
        {
            listingStandard.CheckboxLabeled("RSID_ShowWidgetForShapes.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowWidgetForShapes,
                "RSID_ShowWidgetForShapes.Tooltip".Translate());
        }
        else
        {
            RelevantStatsInDescriptionSettings.ShowWidgetForShapes = true;
        }

        listingStandard.CheckboxLabeled("RSID_UseToolip.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.UseTooltip,
            "RSID_UseToolip.Tooltip".Translate());

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("RSID_CurrentVersion.Label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        //Widgets.EndScrollView();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        RelevantStatsInDescription.ClearCache();
    }
}