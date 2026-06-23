using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Hooks;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class SuperiorSwimmingPower() : PercyJacksonPower, IOnMaxTideChanged
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public async Task OnMaxTideChanged(PlayerChoiceContext choiceContext, Player player, bool fromOverflow = false)
    {
        if (player.Creature != Owner) return;
        var enemies = Owner.CombatState.HittableEnemies;
        foreach (var enemy in enemies)
        {
            await PowerCmd.Apply<SuperiorSwimmingStrengthDownPower>(choiceContext, enemy, Amount, Owner, null);
        }
    }
}