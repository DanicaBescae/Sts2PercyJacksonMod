using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class WaterWalking: PercyJacksonCard
{
    public WaterWalking() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithTide(1);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<WaterWalkingPower>(choiceContext, Owner.Creature, DynamicVars["Tide"].BaseValue,
            Owner.Creature,
            this);
    }
}