using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class FreeTheAquarium: PercyJacksonCard
{
    public FreeTheAquarium() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithCards(1);
        WithVar("Copies", 1, 1);
        WithTide(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue, true);

        var prefs = new CardSelectorPrefs(new LocString("cards", "PERCYJACKSON-FREE_THE_AQUARIUM.selectionPrompt"),
            DynamicVars.Cards.IntValue);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
            (card) => !card.CanPlay(), this)).ToList();

        if (cards.Count == 0)
            return;

        var copiesMade = new CardPileAddResult[cards.Count];

        for (var i = 0; i < cards.Count; i++)
        {
            var cardClone = cards[i].CreateClone();
            cardClone.EnergyCost.SetUntilPlayed(0);
            copiesMade[i] =
                await CardPileCmd.AddGeneratedCardToCombat(cardClone, PileType.Draw, Owner, CardPilePosition.Random);
        }

        CardCmd.PreviewCardPileAdd(copiesMade);
        await Cmd.Wait(0.1f);
    }
}