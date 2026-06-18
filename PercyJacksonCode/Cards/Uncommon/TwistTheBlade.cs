using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class TwistTheBlade : PercyJacksonCard
{
    public TwistTheBlade() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9, 3);
        WithTip(typeof(WoundedPower));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target.HasPower<WoundedPower>())
        {
            await CommonActions.CardAttack(this, play, hitCount: 2).Execute(choiceContext);
        }
        else
        {
            await CommonActions.CardAttack(this, play).Execute(choiceContext);
        }
    }
}