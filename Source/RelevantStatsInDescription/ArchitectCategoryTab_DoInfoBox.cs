using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RelevantStatsInDescription;

[HarmonyPatch(typeof(ArchitectCategoryTab), "DoInfoBox")]
public static class ArchitectCategoryTab_DoInfoBox
{
    private static readonly FieldInfo activeDesignatorSetFieldInfo =
        AccessTools.Field(typeof(Designator_Dropdown), "activeDesignatorSet");

    private static readonly FieldInfo activeDesignatorFieldInfo =
        AccessTools.Field(typeof(Designator_Dropdown), "activeDesignator");

    private static readonly FieldInfo entDefFieldInfo =
        AccessTools.Field(typeof(Designator_Build), "entDef");


    public static void Prefix(ref Rect infoRect)
    {
        if (RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.UseTooltip)
        {
            return;
        }

        var extraHeight = RelevantStatsInDescription.GetExtraHeight();
        if (extraHeight == 0)
        {
            return;
        }

        infoRect.height += extraHeight;
        infoRect.y -= extraHeight;
    }

    public static void Postfix(Rect infoRect, Designator designator)
    {
        if (!RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.UseTooltip)
        {
            return;
        }

        if (designator is Designator_Dropdown designatorDropdown)
        {
            var activeDesignatorSet = (bool)(activeDesignatorSetFieldInfo.GetValue(designatorDropdown) ?? false);
            if (activeDesignatorSet)
            {
                designator = (Designator)activeDesignatorFieldInfo.GetValue(designatorDropdown);
            }
            else
            {
                designator = designatorDropdown.Elements[0];
            }
        }

        if (designator is not Designator_Build buildDesignator)
        {
            return;
        }

        var entDef = (BuildableDef)entDefFieldInfo.GetValue(buildDesignator);
        var toolTip =
            RelevantStatsInDescription.GetUpdatedDescription(entDef, buildDesignator.StuffDef, true);

        if (string.IsNullOrEmpty(toolTip))
        {
            return;
        }

        RelevantStatsInDescription.ShowTooltip(infoRect, toolTip);
    }
}