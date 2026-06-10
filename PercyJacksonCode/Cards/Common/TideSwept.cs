using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class TideSwept: PercyJacksonCard
{
    public TideSwept() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 2);
        WithCards(1);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        var pile = PileType.Hand.GetPile(Owner);
        for (var i = 0; i < DynamicVars["Cards"].IntValue; i++)
        {
            var card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Random);
        }
    }
}