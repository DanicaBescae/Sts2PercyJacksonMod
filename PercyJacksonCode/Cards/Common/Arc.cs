using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class Arc: PercyJacksonCard
{
    public Arc() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(4, 2);
        WithPower<WoundedPower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move)
            .Execute(choiceContext);
        foreach (var enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WoundedPower>(choiceContext, enemy, DynamicVars["WoundedPower"].BaseValue,
                Owner.Creature, this);
        }
    }
}