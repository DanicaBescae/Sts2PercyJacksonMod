using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class BehindYou: PercyJacksonCard
{
    public BehindYou() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6);
        WithPower<VulnerablePower>(1, 1);
        WithPower<WeakPower>(1, 1);
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars["WeakPower"].BaseValue,
            Owner.Creature, this);
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move)
            .Execute(choiceContext);
    }
}