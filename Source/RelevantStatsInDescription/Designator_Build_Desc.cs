using HarmonyLib;
using RimWorld;
using Verse;

namespace RelevantStatsInDescription;

[HarmonyPatch(typeof(Designator_Build), nameof(Designator_Build.Desc), MethodType.Getter)]
public static class Designator_Build_Desc
{
    public static void Postfix(ref string __result, BuildableDef ___entDef, ThingDef ___stuffDef)
    {
        if (RelevantStatsInDescriptionMod.instance.RelevantStatsInDescriptionSettings.UseTooltip)
        {
            return;
        }

        __result = RelevantStatsInDescription.GetUpdatedDescription(___entDef, ___stuffDef);
    }
}