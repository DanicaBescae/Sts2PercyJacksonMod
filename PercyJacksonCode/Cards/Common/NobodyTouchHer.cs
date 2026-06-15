using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class NobodyTouchHer : PercyJacksonCard
{
    public NobodyTouchHer() : base(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
    {
        WithDamage(3, 1);
        WithVar("HitCount", 3);
        WithTide(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var results = (await CommonActions
            .CardAttack(this, play.Target, hitCount: DynamicVars["HitCount"].IntValue, vfx: "vfx/vfx_attack_blunt",
                tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext)).Results;

        var enemies = new HashSet<Creature>();

        foreach (var result in results)
        {
            foreach (var result1 in result)
            {
                _ = enemies.Add(result1.Receiver);
            }
        }

        await TideManager.UpdateTide(Owner, enemies.Count);
    }
}