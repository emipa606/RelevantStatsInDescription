using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RelevantStatsInDescription;

[HarmonyPatch(typeof(ArchitectCategoryTab), nameof(ArchitectCategoryTab.DoInfoBox))]
public static class ArchitectCategoryTab_DoInfoBox
{
    public static void Prefix(ref Rect infoRect)
    {
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.UseTooltip)
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
        if (!RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.UseTooltip)
        {
            return;
        }

        if (designator is Designator_Dropdown designatorDropdown)
        {
            designator = designatorDropdown.activeDesignatorSet
                ? designatorDropdown.activeDesignator
                : designatorDropdown.Elements.First();
        }

        if (designator is not Designator_Build buildDesignator)
        {
            return;
        }

        var toolTip =
            RelevantStatsInDescription.GetUpdatedDescription(buildDesignator.entDef, buildDesignator.StuffDef, true);

        if (string.IsNullOrEmpty(toolTip))
        {
            return;
        }

        ShowTooltip(infoRect, toolTip);
    }

    private static void ShowTooltip(Rect rect, string toolTip)
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