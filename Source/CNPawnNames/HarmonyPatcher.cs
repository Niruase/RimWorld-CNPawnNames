using System.Reflection;
using HarmonyLib;
using Verse;

namespace Niruase.CNPawnNames;

[StaticConstructorOnStartup]
public class HarmonyPatcher
{
    static HarmonyPatcher()
    {
        if (LanguageDatabase.activeLanguage?.folderName != "ChineseSimplified")
        {
            Log.Warning("[CNPawnNames] 未启用：当前语言不是简体中文。 Inactive: current language is not Simplified Chinese.");
            return;
        }

        new Harmony("Niruase.CNPawnNames").PatchAll();
    }
}