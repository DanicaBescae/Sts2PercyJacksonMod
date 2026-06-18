using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class Flood : PercyJacksonCard
{
    public Flood() : base(0, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(10, 3);
        WithCards(3);
        WithTip(typeof(Water));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        var cards = new List<CardModel>();
        for (var i = 0; i < DynamicVars["Cards"].IntValue; i++)
            cards.Add(CombatState.CreateCard<Water>(Owner));
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner);
    }
}