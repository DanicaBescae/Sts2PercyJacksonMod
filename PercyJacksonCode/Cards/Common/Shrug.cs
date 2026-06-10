using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class Shrug: PercyJacksonCard
{
    public Shrug() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(8, 3);
        WithCards(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        await CommonActions.Draw(this, choiceContext);
    }
}