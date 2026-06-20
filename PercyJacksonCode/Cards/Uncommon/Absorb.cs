using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Absorb : PercyJacksonCard
{
    public Absorb() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(TideKeyword);
        WithTip(CardKeyword.Exhaust);
        WithCards(1);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithPower<VigorPower>(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue,
            Owner.Creature, this);
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();
        if (cards.Count == 0)
            return;
        foreach (var card in cards)
        {
            await TideManager.UpdateTide(Owner, card.EnergyCost.Canonical);
            await CardCmd.Exhaust(choiceContext, card);
        }
    }
}