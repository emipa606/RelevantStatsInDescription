using HarmonyLib;
using RimWorld;

namespace RelevantStatsInDescription;

[HarmonyPatch(typeof(Designator_Place), nameof(Designator_Place.DoExtraGuiControls))]
public static class Designator_Place_DoExtraGuiControls
{
    public static bool Prefix(ref float bottomY)
    {
        bottomY -= RelevantStatsInDescription.GetExtraHeight();
        return !RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.RemoveRotateWidget;
    }
}