using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class DeepCuts: PercyJacksonCard
{
    public DeepCuts() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithPower<WoundedPower>(2,1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move)
            .Execute(choiceContext);
        await PowerCmd.Apply<WoundedPower>(choiceContext, play.Target, DynamicVars["WoundedPower"].BaseValue,
            Owner.Creature, this);
    }
}