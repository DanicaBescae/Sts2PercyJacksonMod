using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class AcceleratedBleeding : PercyJacksonCard
{
    public AcceleratedBleeding() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar("HitCount", 2, 1);
        WithTip(typeof(WoundedPower));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (!play.Target.HasPower<WoundedPower>()) return;
        var woundedPower = play.Target.GetPower<WoundedPower>();
        for (var i = 0; i < DynamicVars["HitCount"].IntValue; i++)
        {
            await woundedPower.TriggerWounded(choiceContext, Owner.Creature);
        }

        await PowerCmd.Remove(woundedPower);
    }
}