using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Powers;

public class PermanentScarringPower: PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != CombatSide.Enemy) return;
        var creaturesAffected = combatState.HittableEnemies.ToList();
        
        foreach (var creature in creaturesAffected)
        {
            if (!creature.HasPower<WoundedPower>()) continue;
            var woundedPower = creature.GetPower<WoundedPower>();
            for (var i = 0; i < Amount; i++)
            {
                await woundedPower.TriggerWounded(new ThrowingPlayerChoiceContext(), Owner);
            }
        }
    }
}