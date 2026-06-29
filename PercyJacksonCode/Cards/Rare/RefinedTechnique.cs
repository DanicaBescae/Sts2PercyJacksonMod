using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class RefinedTechnique: PercyJacksonCard
{
    public RefinedTechnique() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithCombo(5, needCombo: true);
        WithDamage(5, 2);
        WithCalculatedVar("HitCount", 0, (c, _) => c.Owner.PlayerCombatState.Combo().CurrentComboCount);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.Damage.BaseValue, ValueProp.Move,
            DynamicVars["HitCount"].IntValue).Execute(choiceContext);
        await ComboManager.ClearCombo(choiceContext, Owner.Creature.CombatState, Owner.PlayerCombatState.Combo());
    }
}