using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class WolfStare: PercyJacksonCard
{
    public WolfStare() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar("BuffAndRemove", 1, 1);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(StrengthPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, play.Target, -1 * DynamicVars["BuffAndRemove"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["BuffAndRemove"].BaseValue,
            Owner.Creature, this);
    }
}