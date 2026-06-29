using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class PiercingStrike : PercyJacksonCard
{
    public PiercingStrike() : base(0, CardType.Attack,
        CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithKeyword(CardKeyword.Innate);
        WithDamage(4);
        WithPower<WoundedPower>(3, 1);
        WithVar("HitCount", 2);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<WoundedPower>(choiceContext, play.Target, DynamicVars["WoundedPower"].BaseValue,
            Owner.Creature, this);
        await CommonActions.CardAttack(this, play, hitCount: DynamicVars["HitCount"].IntValue).Execute(choiceContext);
    }
}