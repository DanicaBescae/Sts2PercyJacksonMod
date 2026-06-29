using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class Precaution : PercyJacksonCard
{
    public Precaution() : base(1, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithBlock(8, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        var prefs = new CardSelectorPrefs(new LocString("cards", "PERCYJACKSON-PRECAUTION.selectionScreenPrompt"),
            1);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();

        if (cards.Count == 0) return;

        foreach (var card in cards)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Random);
        }
    }
}