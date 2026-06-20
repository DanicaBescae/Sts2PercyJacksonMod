using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class OneTwoStab : PercyJacksonCard
{
    public OneTwoStab() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVar("ComboBonus", 3);
        WithCombo(2);
        WithCalculatedDamage(9, (card, _) => IsComboComplete(card) ? card.DynamicVars["ComboBonus"].BaseValue : 0,
            upgrade: 3);
    }

    protected override bool ShouldGlowGoldInternal => IsComboComplete(this);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
}