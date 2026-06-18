using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Powers;

public class TormentedPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public Creature? Tormentor;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Tormentor = applier;
        return Task.CompletedTask;
    }
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext ctx)
    {
        if (Amount <= 0) yield break;

        yield return new HealthBarForecastSegment(
            GetNumDebuffs() * Amount,
            new Color("Gray"),
            HealthBarForecastDirection.FromRight
        );
    }

    private int GetNumDebuffs()
    {
        var debuffs = Owner.Powers.Where(p => p is { Type: PowerType.Debuff, StackType: PowerStackType.Counter });

        return debuffs.Sum(debuff => debuff.Amount);
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side || !participants.Contains(Owner)) return;

        await CreatureCmd.Damage(choiceContext, Owner, GetNumDebuffs() * Amount, ValueProp.Unpowered, Tormentor);
    }

    public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        return creature == Tormentor ? PowerCmd.Remove(this) : Task.CompletedTask;
    }
}