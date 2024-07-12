﻿using Mlie;
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

    //private Vector2 scrollPosition;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public RelevantStatsInDescriptionMod(ModContentPack content) : base(content)
    {
        instance = this;
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.ColumnWidth = (rect.width / 2) - 10f;
        listing_Standard.CheckboxLabeled("RSID_ShowHP.Label".Translate(), ref RelevantStatsInDescriptionSettings.ShowHP,
            "RSID_ShowHP.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.ShowHP)
        {
            listing_Standard.CheckboxLabeled("RSID_ShowHPForAll.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowHPForAll,
                "RSID_ShowHPForAll.Tooltip".Translate());
        }
        else
        {
            RelevantStatsInDescriptionSettings.ShowHPForAll = false;
        }

        listing_Standard.CheckboxLabeled("RSID_ShowAffordance.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowAffordance, "RSID_ShowAffordance.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowAffordanceRequirement.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowAffordanceRequirement,
            "RSID_ShowAffordanceRequirement.Tooltip".Translate());
        if (ModsConfig.IdeologyActive)
        {
            listing_Standard.CheckboxLabeled("RSID_ShowDominantStyle.Label".Translate(),
                ref RelevantStatsInDescriptionSettings.ShowDominantStyle,
                "RSID_ShowDominantStyle.Tooltip".Translate());
        }

        listing_Standard.CheckboxLabeled("RSID_ShowCover.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowCover,
            "RSID_ShowCover.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowBeauty.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowBeauty,
            "RSID_ShowBeauty.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowWealth.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowWealth,
            "RSID_ShowWealth.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowMass.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowMass,
            "RSID_ShowMass.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowDPS.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowDPS,
            "RSID_ShowDPS.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowSize.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowSize,
            "RSID_ShowSize.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowCleanliness.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowCleanliness,
            "RSID_ShowCleanliness.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowTechLevel.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowTechLevel,
            "RSID_ShowTechLevel.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowJoy.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowJoy,
            "RSID_ShowJoy.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowJoyKind.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowJoyKind,
            "RSID_ShowJoyKind.Tooltip".Translate());
        listing_Standard.NewColumn();
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
        listing_Standard.CheckboxLabeled("RSID_ShowResearchSpeed.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowResearchSpeed,
            "RSID_ShowResearchSpeed.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowPowerConsumer.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowPowerConsumer,
            "RSID_ShowPowerConsumer.Tooltip".Translate());
        if (RelevantStatsInDescriptionSettings.ShowPowerConsumer && (RelevantStatsInDescription.RepowerOnOffLoaded ||
                                                                     RelevantStatsInDescription.LightsOutLoaded))
        {
            listing_Standard.Label("RSID_VariedPowerModLoaded.Label".Translate(), -1f,
                "RSID_VariedPowerModLoaded.Tooltip".Translate());
        }

        listing_Standard.CheckboxLabeled("RSID_ShowPowerProducer.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowPowerProducer,
            "RSID_ShowPowerProducer.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowStorageSpace.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowStorageSpace,
            "RSID_ShowStorageSpace.Tooltip".Translate());
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

        listing_Standard.CheckboxLabeled("RSID_ShowFloorQuality.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowFloorQuality,
            "RSID_ShowFloorQuality.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowDefName.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowDefName,
            "RSID_ShowDefName.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_ShowUIOrder.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.ShowUIOrder,
            "RSID_ShowUIOrder.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_RemoveRotateWidget.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.RemoveRotateWidget,
            "RSID_RemoveRotateWidget.Tooltip".Translate());
        listing_Standard.CheckboxLabeled("RSID_UseToolip.Label".Translate(),
            ref RelevantStatsInDescriptionSettings.UseTooltip,
            "RSID_UseToolip.Tooltip".Translate());

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("RSID_CurrentVersion.Label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        //Widgets.EndScrollView();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        RelevantStatsInDescription.ClearCache();
    }
}