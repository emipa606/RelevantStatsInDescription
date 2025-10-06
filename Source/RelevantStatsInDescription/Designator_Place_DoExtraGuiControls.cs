using System.Linq;
using HarmonyLib;
using RimWorld;

namespace RelevantStatsInDescription;

[HarmonyPatch(typeof(Designator_Place), nameof(Designator_Place.DoExtraGuiControls))]
public static class Designator_Place_DoExtraGuiControls
{
    public static bool Prefix(ref float bottomY, Designator_Place __instance)
    {
        bottomY -= RelevantStatsInDescription.GetExtraHeight();
        if (!RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.RemoveRotateWidget)
        {
            return true;
        }

        if (!RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.ShowWidgetForShapes)
        {
            return false;
        }

        return __instance.DrawStyleCategory?.styles?.Any() == true;
    }
}