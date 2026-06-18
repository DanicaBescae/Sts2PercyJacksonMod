using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class EbbAndFlow : PercyJacksonCard
{
    public EbbAndFlow() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(2);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var discardPrefs = new CardSelectorPrefs(new LocString("cards", "PERCYJACKSON-EBB_AND_FLOW.discardPrompt"),
            DynamicVars.Cards.IntValue);
        var discardedCards = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, discardPrefs);
        await CardPileCmd.Add(discardedCards, PileType.Draw, CardPilePosition.Random);
        
        var drawPrefs = new CardSelectorPrefs(new LocString("cards", "PERCYJACKSON-EBB_AND_FLOW.drawPrompt"),
            DynamicVars.Cards.IntValue);
        var drawCards = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, drawPrefs);
        await CardPileCmd.Add(drawCards, PileType.Discard);
    }
}