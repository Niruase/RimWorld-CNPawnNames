using HarmonyLib;
using Verse;

namespace Niruase.CNPawnNames;

[StaticConstructorOnStartup]
public class HarmonyPatcher
{
    static HarmonyPatcher()
    {
        if (LanguageDatabase.activeLanguage?.folderName != "ChineseSimplified (简体中文)")
        {
            Log.Warning("[CNPawnNames] 未启用：当前语言不是简体中文。 Inactive: current language is not Simplified Chinese.");
            Log.Warning($"[CNPawnNames] 当前语言：{LanguageDatabase.activeLanguage?.folderName ?? "未加载"}");

            return;
        }

        new Harmony("Niruase.CNPawnNames").PatchAll();
        Log.Message("[CNPawnNames] 已启用。Loaded.");
    }
}