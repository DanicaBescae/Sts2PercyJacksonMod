using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class CaptureTheFlag: PercyJacksonCard
{
    public CaptureTheFlag() : base(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
    {
        WithDamage(3);
        WithTide(1);
        WithVar("HitCount", 3, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        HashSet<Creature> enemiesHit = [];
        for (var i = 0; i < DynamicVars["HitCount"].IntValue; i++)
        {
            var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (enemy == null)
                return;
            enemiesHit.Add(enemy);
            await CreatureCmd.Damage(choiceContext, enemy, DynamicVars.Damage, this);
        }

        await TideManager.UpdateTide(Owner, enemiesHit.Count);
    }
}