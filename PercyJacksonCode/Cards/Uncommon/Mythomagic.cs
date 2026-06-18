using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Mythomagic: PercyJacksonCard
{
    public Mythomagic() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(3, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var pile = PileType.Hand.GetPile(Owner);
        var card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (card != null)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Random);
        }
        await CommonActions.Draw(this, choiceContext);
    }
}