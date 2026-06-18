using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;

namespace PercyJackson.PercyJacksonCode.Models;

public class CombatManagerPlus(): CustomSingletonModel(HookType.Combat)
{
    private static Dictionary<Player, int> timesShuffledThisCombat = new();

    public static int GetTimesShuffledThisCombat(Player player)
    {
        return timesShuffledThisCombat.GetValueOrDefault(player, 0);
    }

    public override Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (!timesShuffledThisCombat.TryAdd(shuffler, 1)) timesShuffledThisCombat[shuffler]++;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        timesShuffledThisCombat.Clear();
        return Task.CompletedTask;
    }
}