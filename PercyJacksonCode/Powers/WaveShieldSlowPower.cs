using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class WaveShieldSlowPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    private SlowPower? _slowPower;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.HasPower<SlowPower>()) await PowerCmd.Remove(this);
        await PowerCmd.Apply<SlowPower>(new ThrowingPlayerChoiceContext(), Owner, 1, applier, cardSource);
        _slowPower = Owner.GetPower<SlowPower>();
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner.Side == side) return;
        await PowerCmd.Remove(_slowPower);
        await PowerCmd.Remove(this);
    }
}