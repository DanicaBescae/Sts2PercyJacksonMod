using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class DoubleEdgedSword: PercyJacksonCard
{
    public DoubleEdgedSword() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(4, 2);
        WithVar("HitCount", 2);
        WithPower<StrengthPower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<DoubledEdgedSwordPower>(choiceContext, Owner.Creature,
            DynamicVars["StrengthPower"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<DoubledEdgedSwordPower>(choiceContext, play.Target, DynamicVars["StrengthPower"].BaseValue,
            Owner.Creature, this);
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move,
            hitCount: DynamicVars["HitCount"].IntValue).Execute(choiceContext);
    }
}