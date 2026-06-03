using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;

namespace PercyJackson.PercyJacksonCode.Models;

public class TideManager(): CustomSingletonModel(HookType.Combat)
{
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        return side != CombatSide.Player ? Task.CompletedTask : ApplyVigorFromTideToAll(choiceContext, combatState.Players);
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return Task.CompletedTask;
        foreach (var player in participants.LastOrDefault().CombatState.Players)
        {
            ClearTempMaxTide(player);
        }

        return Task.CompletedTask;
    }

    private static async Task ApplyVigorFromTideToAll(PlayerChoiceContext choiceContext, IReadOnlyList<Player> players)
    {
        foreach (var player in players)
        {
            if (player.PlayerCombatState.Tide().CurrentTide == 0) continue;
            var tide = player.PlayerCombatState.Tide().CurrentTide;
            await PowerCmd.Apply<VigorPower>(choiceContext, player.Creature, tide, player.Creature, null);
        }
    }

    public static async Task UpdateTide(Player player, int tideChange, bool negative = false)
    {
        if(!negative) RaiseTide(player, tideChange);
        else await LowerTide(player, tideChange);
    }

    public static void UpdateMaxTide(Player player, int tideChange, bool temporary = false)
    {
        if (temporary)
            player.PlayerCombatState.Tide().TempMaxTide = player.PlayerCombatState.Tide().MaxTide + tideChange;
        else player.PlayerCombatState.Tide().MaxTide += tideChange;
    }

    private static Task ClearTempMaxTide(Player player)
    {
        player.PlayerCombatState.Tide().TempMaxTide = 0;
        return Task.CompletedTask;
    }

    private static void RaiseTide(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        if (tideCombatState.CurrentTide + tideChange > tideCombatState.MaxTide) ApplyStrengthFromTide(player, tideChange);
        else player.PlayerCombatState.Tide().CurrentTide += tideChange;
    }

    private static async Task LowerTide(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        if (tideCombatState.CurrentTide - tideChange < 0) await LoseHpFromTide(player, tideChange);
        else player.PlayerCombatState.Tide().CurrentTide -= tideChange;
    }
    
    private static async Task LoseHpFromTide(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        var tide = (int)tideCombatState.CurrentTide + tideChange;
        for (var i = 0; i < tide; i++)
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), player.Creature,
                new DamageVar(2, ValueProp.Unblockable), player.Creature);
        }
    }

    private static void ApplyStrengthFromTide(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        var tide = tideCombatState.CurrentTide;
        PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player.Creature, 1, player.Creature, null);
        tideCombatState.CurrentTide = (tide + tideChange) % tideCombatState.MaxTide;
    }
}