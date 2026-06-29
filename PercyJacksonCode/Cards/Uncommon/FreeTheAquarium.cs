using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class FreeTheAquarium: PercyJacksonCard
{
    public FreeTheAquarium() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(CardKeyword.Exhaust);
        WithCards(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
            (card) => !card.CanPlay(), this)).ToList();

        if (cards.Count == 0)
            return;

        CardPileAddResult[] copiesMade = [];

        foreach (var c in cards)
        {
            if (c.Type is not (CardType.Attack or CardType.Skill)) continue;
            var cardClone = c.CreateClone();
            cardClone.EnergyCost.SetUntilPlayed(0);
            _ = copiesMade.Append(CardPileCmd
                .AddGeneratedCardToCombat(cardClone, PileType.Draw, Owner, CardPilePosition.Random).Result);
        }

        CardCmd.PreviewCardPileAdd(copiesMade);
        if (copiesMade.Length != 0 && !IsUpgraded) await CardCmd.Exhaust(choiceContext, this);
    }
}