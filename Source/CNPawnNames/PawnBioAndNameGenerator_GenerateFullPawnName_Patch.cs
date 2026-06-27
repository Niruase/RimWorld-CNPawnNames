using HarmonyLib;
using RimWorld;
using Verse;

namespace Niruase.CNPawnNames;

[HarmonyPatch(typeof(PawnBioAndNameGenerator), "GenerateFullPawnName")]
public static class PawnBioAndNameGenerator_GenerateFullPawnName_Patch
{
    private const int MaxDedupAttempts = 50;

    static bool Prefix(
        ref Name __result,
        bool creepjoiner,
        Gender gender,
        PawnNameCategory nameCategory,
        string forcedLastName,
        bool forceNoNick)
    {
        // Only intercept creepjoiners — normal pawns run the original method
        if (!creepjoiner)
            return true;

        // First name: standard NameBank (your existing NameBank_GetName patch handles Chinese first names)
        NameBank nameBank = PawnNameDatabaseShuffled.BankOf(nameCategory);
        string first = nameBank.GetName(PawnNameSlot.First, gender);

        // Last name: creepjoiner-specific translated pool
        string last = forcedLastName ?? NameTables.CreepLastPool.RandomElement();

        // Nickname: ~50% chance to include one (matches vanilla 2-rule variant split)
        string nick = null;
        if (!forceNoNick && Rand.Value < 0.5f)
        {
            nick = NameTables.CreepNickPool.RandomElement();
        }

        // Deduplication: re-roll nick + last up to 50 times if full name was already used this game
        // (Matches vanilla NameResolvedFrom behavior which checks UsedThisGame on full NameTriple)
        NameTriple result = new NameTriple(first, nick, last);
        int attempts = 0;
        while (attempts < MaxDedupAttempts && result.UsedThisGame)
        {
            attempts++;
            last = forcedLastName ?? NameTables.CreepLastPool.RandomElement();

            if (!forceNoNick)
            {
                if (Rand.Value < 0.5f)
                {
                    nick = NameTables.CreepNickPool.RandomElement();
                }
                else
                {
                    nick = null;
                }
            }

            result = new NameTriple(first, nick, last);
        }

        __result = result;
        return false; // Skip original method entirely
    }
}