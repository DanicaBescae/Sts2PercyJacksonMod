using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class SaveForLater: PercyJacksonCard
{
    public SaveForLater() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(13, 3);
        WithCards(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move).Execute(choiceContext);
        
        var prefs = new CardSelectorPrefs(new LocString("cards", "PERCYJACKSON-SAVE_FOR_LATER.selectionPrompt"),
            DynamicVars.Cards.IntValue);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();

        if (cards.Count == 0)
            return;

        foreach (var card in cards)
        {
            await CardPileCmd.Add(card, PileType.Draw);
        }
    }
}