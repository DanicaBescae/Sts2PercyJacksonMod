using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class LifeBuoy: PercyJacksonCard
{
    public LifeBuoy() : base(1, CardType.Attack, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 3);
        WithCards(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card != this) return;
        if (Pile.Type != PileType.Draw || oldPileType == PileType.Draw) return;
        await CommonActions.Draw(this, new BlockingPlayerChoiceContext());
    }
}