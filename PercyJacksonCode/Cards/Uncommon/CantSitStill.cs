using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class CantSitStill : PercyJacksonCard
{
    public CantSitStill() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(4, 2);
        WithCalculatedVar("HitCount", 0,
            (card, target) => CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>()
                .Count(e =>
                    e.Receiver == target && e.Result.Props.IsPoweredAttack() && e.HappenedThisTurn(card.CombatState) &&
                    e.Dealer != null && e.Dealer == card.Owner.Creature));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, hitCount: DynamicVars["HitCount"].IntValue).Execute(choiceContext);
    }
}