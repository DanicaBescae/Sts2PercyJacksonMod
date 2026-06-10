using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class WoundedPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext ctx)
    {
        if (Amount <= 0) yield break;

        yield return new HealthBarForecastSegment(
            Amount,
            new Color("Red"),
            HealthBarForecastDirection.FromRight
        );
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage == 0 || !props.IsPoweredAttack() || dealer == Owner) return;

        await TriggerWounded(choiceContext, dealer);
        await PowerCmd.Decrement(this);
    }

    public async Task TriggerWounded(PlayerChoiceContext choiceContext, Creature dealer)
    {
        await CreatureCmd.Damage(choiceContext, Owner, Amount, ValueProp.Unpowered, dealer);
    }
}