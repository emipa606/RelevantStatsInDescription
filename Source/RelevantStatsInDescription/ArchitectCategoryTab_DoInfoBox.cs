using System;
using System.Linq;
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
            var activeDesignatorSet = (bool)activeDesignatorSetFieldInfo.GetValue(designatorDropdown);
            if (activeDesignatorSet)
            {
                designator = (Designator)activeDesignatorFieldInfo.GetValue(designatorDropdown);
            }
            else
            {
                designator = designatorDropdown.Elements.First();
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

        showTooltip(infoRect, toolTip);
    }

    private static void showTooltip(Rect rect, string toolTip)
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