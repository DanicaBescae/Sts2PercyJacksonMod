using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisTurn))]
internal class CardModelSetToFreeThisTurnPatch
{
    [HarmonyPostfix]
    private static void Postfix(CardModel __instance)
    {
        if (__instance is PercyJacksonCard card && ComboManager.IsComboChainCard(__instance)) card.SetComboThisTurn(0);
    }
}


[HarmonyPatch(typeof(CardModel), nameof(CardModel.EndOfTurnCleanup))]
internal class CardModelEndOfTurnCleanupPatch
{
    [HarmonyPostfix]
    private static void Postfix(CardModel __instance)
    {
        if (__instance is PercyJacksonCard card)
        {
            card.RemoveEndOfTurnCombo();
        }
    }
}
