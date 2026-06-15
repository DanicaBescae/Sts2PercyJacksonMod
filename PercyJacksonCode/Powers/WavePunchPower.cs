using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;

namespace PercyJackson.PercyJacksonCode.Powers;

public class WavePunchPower: PercyJacksonPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;

        var hittableEnemies = CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0)
            return;

        var tide = player.PlayerCombatState.Tide();
        if (tide == null) return;

        Flash();
        await CreatureCmd.Damage(choiceContext,
            Owner.Player.RunState.Rng.CombatTargets.NextItem(hittableEnemies),
            Amount * tide.TideGainedThisCombat, ValueProp.Unpowered, Owner, null);
    }
}