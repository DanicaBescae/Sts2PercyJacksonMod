using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class QuickParry : PercyJacksonCard
{
    public QuickParry() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(4, 6);
        WithCombo(1, 1, true);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsComboComplete(this))
        {
            await CommonActions.CardBlock(this, play);
        }
    }
}