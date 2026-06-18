using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class TetanusPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    private const int ApplicationsNeeded = 3;
    private int _timesApplied;

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner || amount <= 0 || power is not WoundedPower) return Task.CompletedTask;
        _timesApplied++;
        
        if (_timesApplied < ApplicationsNeeded) return Task.CompletedTask;
        
        PowerCmd.Apply<PoisonPower>(choiceContext, power.Owner, amount, applier, cardSource);
        _timesApplied = 0;
        return Task.CompletedTask;
    }
}