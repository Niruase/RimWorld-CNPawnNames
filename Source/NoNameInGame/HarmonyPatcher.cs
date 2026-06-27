using System.Reflection;
using HarmonyLib;
using Verse;

namespace Niruase.NoNameInGame;

[StaticConstructorOnStartup]
public class HarmonyPatcher
{
    static HarmonyPatcher()
    {
        new Harmony("rimworld.niruase.nonameingame").PatchAll(Assembly.GetExecutingAssembly());
    }
}