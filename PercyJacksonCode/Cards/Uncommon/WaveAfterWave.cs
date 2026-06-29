using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class WaveAfterWave : PercyJacksonCard
{
    public WaveAfterWave() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(6);
        WithCards(2, 3);
        WithCalculatedVar("HitCount", 0,
            (c, _) => Math.Min(CardPile.MaxCardsInHand,
                PileType.Hand.GetPile(c.Owner).Cards.Count + c.DynamicVars.Cards.IntValue));
        WithTip(typeof(Water));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            var water = CombatState.CreateCard<Water>(Owner);
            await CardPileCmd.Add(water, PileType.Hand);
        }

        await CommonActions
            .CardAttack(this, play,
                hitCount: Math.Min(CardPile.MaxCardsInHand, PileType.Hand.GetPile(Owner).Cards.Count))
            .Execute(choiceContext);
    }
}