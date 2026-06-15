using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Token;

public class Water: PercyJacksonCard
{
    public Water() : base(-1, CardType.Status, CardRarity.Token, TargetType.Self)
    {
        WithKeyword(CardKeyword.Unplayable);
        WithKeyword(CardKeyword.Exhaust);
        WithCards(2, 1);
        WithVar("Discard", 2);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return;
        await Activate(choiceContext);
        await CardCmd.Exhaust(choiceContext, this);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card != this) return;
        await Activate(choiceContext);
    }

    private async Task Activate(PlayerChoiceContext choiceContext)
    {
        await CommonActions.Draw(this, choiceContext);
        await CardCmd.Discard(choiceContext,
            await CardSelectCmd.FromHandForDiscard(choiceContext, Owner,
                new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, DynamicVars["Discard"].IntValue), null,
                this));
    }
}