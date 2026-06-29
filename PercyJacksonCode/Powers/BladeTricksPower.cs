using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class BladeTricksPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;
    
    private const int ComboCardsBeforeRetain = 2;

    private int comboCardsPlayed = 0;

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Owner != Owner.Player || !ComboManager.IsComboCard(card)) return Task.CompletedTask;
        comboCardsPlayed++;
        if (comboCardsPlayed % ComboCardsBeforeRetain == 0 && !card.Keywords.Contains(CardKeyword.Retain))
            card.AddKeyword(CardKeyword.Retain);
        return Task.CompletedTask;
    }
}