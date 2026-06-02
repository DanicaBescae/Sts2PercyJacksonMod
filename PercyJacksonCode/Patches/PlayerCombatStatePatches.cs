using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using PercyJackson.PercyJacksonCode.Fields;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Patches;

public class PlayerCombatStatePatches
{
    [HarmonyPatch(typeof(PlayerCombatState), MethodType.Constructor)]
    [HarmonyPatch([typeof(Player)])]
    internal class PlayerCombatStateConstructorPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Player player, PlayerCombatState __instance)
        {
            PercyJacksonFields.TideCombatState[__instance] = new TideCombatState
            {
                Owner = player
            };
        }
    }
}