using HarmonyLib;
using RimWorld;

namespace Niruase.CNPawnNames;

[HarmonyPatch(typeof(PawnBioAndNameGenerator), "TryGiveSolidBioTo")]
public static class PawnBioAndNameGenerator_TryGiveSolidBioTo_Patch
{
    static bool Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
}