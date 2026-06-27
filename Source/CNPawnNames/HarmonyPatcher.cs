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
            Log.Warning("[CNPawnNames] 未启用：当前语言不是简体中文。\nDeactivated: current language is not Simplified Chinese.");
            Log.Message($"[CNPawnNames] 当前语言文件夹：{LanguageDatabase.activeLanguage?.folderName ?? "未加载"}。\nLanguage folder: {LanguageDatabase.activeLanguage?.folderName ?? "not loaded"}.");

            return;
        }

        new Harmony("Niruase.CNPawnNames").PatchAll();
        Log.Message("[CNPawnNames] 已启用。Activated.");
    }
}