using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class OpenedSores: PercyJacksonCard
{
    public OpenedSores() : base(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
    {
        WithPower<WoundedPower>(4, 2);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<WoundedPower>(choiceContext, play.Target, DynamicVars["WoundedPower"].BaseValue,
            Owner.Creature, this);
    }
}