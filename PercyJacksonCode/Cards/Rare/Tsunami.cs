using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Tsunami : PercyJacksonCard
{
    public Tsunami() : base(1, CardType.Attack,
        CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVar("BonusDamage", 3);
        WithCalculatedDamage(6, (card, _) =>
        {
            if (!card.IsUpgraded)
            {
                return card.DynamicVars["BonusDamage"].BaseValue *
                       PileType.Exhaust.GetPile(card.Owner).Cards.Count(c => c is Water);
            }

            return card.DynamicVars["BonusDamage"].BaseValue *
                   (PileType.Exhaust.GetPile(card.Owner).Cards.Count(c => c is Water) +
                    PileType.Draw.GetPile(card.Owner).Cards.Count(c => c is Water));
        });
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }
}