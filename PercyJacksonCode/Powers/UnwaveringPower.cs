using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Powers;

public class UnwaveringPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private int _timesAlreadyApplied;

    private Task ReapplyVigor(PlayerChoiceContext choiceContext, int amt)
    {
        if (_timesAlreadyApplied >= Amount) return Task.CompletedTask;
        Flash();
        PowerCmd.Apply<VigorPower>(choiceContext, Owner, amt, Owner, null);
        _timesAlreadyApplied++;
        return Task.CompletedTask;
    }

    [HarmonyPatch(typeof(VigorPower), nameof(VigorPower.AfterAttack))]
    internal class VigorPowerAfterAttackPatch
    {
        [HarmonyPostfix]
        private static void Postfix(VigorPower __instance, PlayerChoiceContext choiceContext, AttackCommand command)
        {
            var blessingPower = __instance.Owner.GetPower<UnwaveringPower>();
            if (blessingPower == null) return;

            var data = __instance.GetInternalData<VigorPower.Data>();
            
            if (command == data.commandToModify)
            {
                blessingPower.ReapplyVigor(choiceContext, data.amountWhenAttackStarted);
            }
        }
    }
}