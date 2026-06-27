using HarmonyLib;
using RimWorld;
using Verse;

namespace Niruase.CNPawnNames;

// 正确：目标类是 NameBank，方法是 NameBank.GetName
[HarmonyPatch(typeof(NameBank), nameof(NameBank.GetName))]
public static class NameBank_GetName_Patch
{
    // 实例方法补丁，__instance 可选，这里保留以符合规范
    static bool Prefix(NameBank __instance, ref string __result, PawnNameSlot slot, Gender gender)
    {
        // First names: gender-aware selection
        if (slot == PawnNameSlot.First)
        {
            __result = gender == Gender.Male 
                ? NameTables.MaleFirstNames.Values.RandomElement()
                : NameTables.FemaleFirstNames.Values.RandomElement();
            return false; // Skip original method entirely
        }
        
        // Last names: any surname
        if (slot == PawnNameSlot.Last)
        {
            __result = NameTables.LastNames.Values.RandomElement();
            return false;
        }
        
        // Nicknames
        if (slot == PawnNameSlot.Nick)
        {
            __result = NameTables.Nicknames.Values.RandomElement();
            return false;
        }

        // Fallback: unknown slot type, run original method
        return true;
    }
}