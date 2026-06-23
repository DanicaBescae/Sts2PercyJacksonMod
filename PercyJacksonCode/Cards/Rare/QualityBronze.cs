using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class QualityBronze : PercyJacksonCard
{
    public QualityBronze() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<WoundedPower>(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<QualityBronzePower>(choiceContext, Owner.Creature, DynamicVars["WoundedPower"].BaseValue,
            Owner.Creature, this);
    }
}