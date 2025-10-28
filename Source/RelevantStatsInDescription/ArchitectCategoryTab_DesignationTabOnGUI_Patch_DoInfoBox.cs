using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RelevantStatsInDescription;

[HarmonyPatch]
public static class ArchitectCategoryTab_DesignationTabOnGUI_Patch_DoInfoBox
{
    private static readonly FieldInfo activeDesignatorSetFieldInfo =
        AccessTools.Field(typeof(Designator_Dropdown), "activeDesignatorSet");

    private static readonly FieldInfo activeDesignatorFieldInfo =
        AccessTools.Field(typeof(Designator_Dropdown), "activeDesignator");

    private static readonly FieldInfo entDefFieldInfo =
        AccessTools.Field(typeof(Designator_Build), "entDef");

    public static bool Prepare()
    {
        return ModLister.GetActiveModWithIdentifier("ferny.BetterArchitect", true) != null;
    }

    public static MethodBase TargetMethod()
    {
        var architectCategoryTabType =
            AccessTools.TypeByName("BetterArchitect.ArchitectCategoryTab_DesignationTabOnGUI_Patch");
        return AccessTools.Method(architectCategoryTabType, "DoInfoBox");
    }

    public static void Postfix(Designator designator)
    {
        if (!RelevantStatsInDescriptionMod.Instance.RelevantStatsInDescriptionSettings.UseTooltip)
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

        RelevantStatsInDescription.ShowTooltip(ArchitectCategoryTab.InfoRect, toolTip);
    }
}