using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Character;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class UnexpectedStrategy : PercyJacksonCard
{
    public UnexpectedStrategy() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithTip(ComboStarter);
        WithKeyword(ComboKeyword);
        WithKeyword(CardKeyword.Exhaust);
        WithCards(1);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var prefs = new CardSelectorPrefs(new LocString("cards", "PERCYJACKSON-UNEXPECTED_STRATEGY.selectionPrompt"),
            DynamicVars.Cards.IntValue);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
            (card) => !card.Keywords.Contains(ComboKeyword), this)).ToList();
        
        if (cards.Count == 0)
            return;
        
        foreach (var card in cards)
        {
            CardCmd.ApplyKeyword(card, ComboStarter);
            CardCmd.ApplyKeyword(card, ComboKeyword);
        }
    }
}