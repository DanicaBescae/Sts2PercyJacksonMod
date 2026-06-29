using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Models;

public class CombatManagerPlus(): CustomSingletonModel(HookType.Combat)
{
    // Note: in the future should turn these into SpireFields, extend combatmanager, or turn into combathistoryentries
    // for more flexibility
    private static readonly Dictionary<Player, int> TimesShuffledThisCombat = new();
    private static readonly Dictionary<Player, int> MultiHitsPlayedThisCombat = new();

    public static int GetTimesShuffledThisCombat(Player player)
    {
        return TimesShuffledThisCombat.GetValueOrDefault(player, 0);
    }
    
    public static int GetMultiHitsPlayedThisCombat(Player player)
    {
        return MultiHitsPlayedThisCombat.GetValueOrDefault(player, 0);
    }

    public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker?.Player is null || command.DamageProps != ValueProp.Move || command.Results.Count() <= 1)
            return Task.CompletedTask;
        MultiHitsPlayedThisCombat[command.Attacker.Player]++;
        return Task.CompletedTask;
    }

    public override Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (!TimesShuffledThisCombat.TryAdd(shuffler, 1)) TimesShuffledThisCombat[shuffler]++;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        TimesShuffledThisCombat.Clear();
        return Task.CompletedTask;
    }
}