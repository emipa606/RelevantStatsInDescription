using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RelevantStatsInDescription;

[HarmonyPatch]
public static class ArchitectCategoryTab_InfoRect
{
    public static bool Prepare()
    {
        return ModLister.GetActiveModWithIdentifier("ferny.BetterArchitect", true) != null;
    }

    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(ArchitectCategoryTab), "get_InfoRect");
    }

    public static void Postfix(ref Rect __result)
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

        __result.height += extraHeight;
        __result.y -= extraHeight;
    }
}