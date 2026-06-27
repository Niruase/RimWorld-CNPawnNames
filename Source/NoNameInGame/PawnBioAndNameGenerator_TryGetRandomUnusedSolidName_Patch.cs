using HarmonyLib;
using RimWorld;
using Verse;

namespace Niruase.NoNameInGame;

[HarmonyPatch(typeof(PawnBioAndNameGenerator), "TryGetRandomUnusedSolidName")]
public static class PawnBioAndNameGenerator_TryGetRandomUnusedSolidName_Patch
{
    static bool Prefix(ref NameTriple __result)
    {
        __result = null;
        return false;
    }
}