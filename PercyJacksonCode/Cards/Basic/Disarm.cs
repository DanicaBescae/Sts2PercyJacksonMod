using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Basic;

public class Disarm : PercyJacksonCard
{
    public Disarm() : base(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithBlock(5, 7);
        WithVar("StrengthLoss", 1, 1);
        WithCombo(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        if (IsComboComplete(this))
        {
            await PowerCmd.Apply<DarkShacklesPower>(choiceContext, play.Target, DynamicVars["StrengthLoss"].BaseValue,
                Owner.Creature, this);
        }
    }
}