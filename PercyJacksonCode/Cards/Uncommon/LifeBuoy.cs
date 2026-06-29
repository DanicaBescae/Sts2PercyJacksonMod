using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class LifeBuoy: PercyJacksonCard
{
    public LifeBuoy() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(8, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        if (Keywords.Contains(CardKeyword.Exhaust) || ExhaustOnNextPlay)
            return;
        _ = await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top);
    }
}