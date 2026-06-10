using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Powers;

public class GreekStyleImprovPower: PercyJacksonPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if(side != CombatSide.Player) return;
        await PowerCmd.Apply<ImprovisingPower>(new ThrowingPlayerChoiceContext(), Owner, 1, Owner, null);
        await PowerCmd.Decrement(this);
    }
}