using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class Backswing : PercyJacksonCard
{
    public Backswing() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(9, 3);
        WithCombo(2);
        WithCards(1);
    }

    protected override bool ShouldGlowGoldInternal => IsComboComplete(this);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (IsComboComplete(this))
        {
            var cardsToDraw = DynamicVars.Cards.IntValue * Owner.PlayerCombatState.Combo().CurrentComboCount;
            await CardPileCmd.Draw(choiceContext, cardsToDraw, Owner);
        }
    }
}