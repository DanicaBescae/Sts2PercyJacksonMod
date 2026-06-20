using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SafeSailing : PercyJacksonCard
{
    public SafeSailing() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(8, 3);
    }

    protected override bool ShouldGlowGoldInternal => CombatManagerPlus.GetCardsDrawnThisTurn(Owner) > 0;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        if (CombatManagerPlus.GetCardsDrawnThisTurn(Owner) > 0) await CommonActions.CardBlock(this, play);
    }
}