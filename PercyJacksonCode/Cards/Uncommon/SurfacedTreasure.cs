using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SurfacedTreasure : PercyJacksonCard
{
    public SurfacedTreasure() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(1);
        WithTip(typeof(Water));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SurfacedTreasurePower>(choiceContext, Owner.Creature, DynamicVars.Cards.BaseValue,
            Owner.Creature, this);
        if (IsUpgraded)
        {
            var water = CombatState.CreateCard<Water>(Owner);
            await CardPileCmd.Add(water, PileType.Draw, CardPilePosition.Random);
        }
    }
}