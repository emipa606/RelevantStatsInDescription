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
        var extraRows = RelevantStatsInDescription.GetExtraHeight(designator);
        if (extraRows == 0)
        {
            return;
        }

        infoRect.height += extraRows * 25f;
        infoRect.y -= extraRows * 25f;
    }
}