using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class TideSwept: PercyJacksonCard
{
    public TideSwept() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(8, 3);
        WithTip(typeof(Water));
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        var water = CombatState.CreateCard<Water>(Owner);
        await CardPileCmd.Add(water, PileType.Draw, CardPilePosition.Top);
    }
}