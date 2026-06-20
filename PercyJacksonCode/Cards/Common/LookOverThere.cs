using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class LookOverThere: PercyJacksonCard
{
    public LookOverThere() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithComboStarter();
        WithCards(1, 1);
        WithPower<VulnerablePower>(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);
        await CommonActions.Draw(this, choiceContext);
    }
}