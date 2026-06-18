using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class SwordMastery : PercyJacksonCard
{
    public SwordMastery() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<SwordMasteryPower>(5);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SwordMasteryPower>(choiceContext, Owner.Creature,
            DynamicVars["SwordMasteryPower"].BaseValue, Owner.Creature, this);
    }
}