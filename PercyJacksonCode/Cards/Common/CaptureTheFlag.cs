using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class CaptureTheFlag: PercyJacksonCard
{
    public CaptureTheFlag() : base(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
    {
        WithDamage(5,7);
        WithTide(1);
        WithVar("HitCount", 1);
        WithVar("HitIncrease", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < (int)DynamicVars["HitCount"].BaseValue; i++)
        {
            var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (enemy == null)
                return;
            await CommonActions.CardAttack(this, enemy, DynamicVars.Damage.BaseValue, ValueProp.Move).Execute(choiceContext);
            await TideManager.UpdateTide(Owner, 1);
        }
        DynamicVars["HitCount"].BaseValue += DynamicVars["HitIncrease"].IntValue;
    }
}