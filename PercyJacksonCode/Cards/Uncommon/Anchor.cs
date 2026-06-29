using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Anchor : PercyJacksonCard
{
    public Anchor() : base(4, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(36, 42);
        WithVar("EnergyDecrease", 1);
        WithTip(CardKeyword.Unplayable);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (!card.Keywords.Contains(CardKeyword.Unplayable)) return Task.CompletedTask;
        EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }
}