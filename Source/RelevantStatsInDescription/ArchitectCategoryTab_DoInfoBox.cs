using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RelevantStatsInDescription;

[HarmonyPatch(typeof(ArchitectCategoryTab), "DoInfoBox")]
public static class ArchitectCategoryTab_DoInfoBox
{
    public static void Prefix(ref Rect infoRect, Designator designator)
    {
        var extraHeight = RelevantStatsInDescription.GetExtraHeight();
        if (extraHeight == 0)
        {
            return;
        }

        infoRect.height += extraHeight;
        infoRect.y -= extraHeight;
    }
}