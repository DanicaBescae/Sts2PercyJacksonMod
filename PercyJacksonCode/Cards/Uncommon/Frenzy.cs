using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Frenzy : PercyJacksonCard
{
    public Frenzy() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(4, 1);
        WithCalculatedVar("HitCount", 0, (c, _) => CombatManagerPlus.GetMultiHitsPlayedThisCombat(c.Owner));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, hitCount: DynamicVars["HitCount"].IntValue).Execute(choiceContext);
    }
}