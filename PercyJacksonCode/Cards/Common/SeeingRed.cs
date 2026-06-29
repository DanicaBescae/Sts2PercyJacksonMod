using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class SeeingRed : PercyJacksonCard
{
    public SeeingRed() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(2, 1);
        WithVar("HitCount", 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, hitCount: DynamicVars["HitCount"].IntValue).Execute(choiceContext);
    }
}