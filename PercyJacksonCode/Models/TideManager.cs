using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Hooks;

namespace PercyJackson.PercyJacksonCode.Models;

public class TideManager(): CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player) return;
        await ApplyVigorFromTideToAll(choiceContext, combatState.Players);
        foreach (var player in participants.OfType<Player>())
        {
            await ClearTempMaxTide(player);
        }
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return Task.CompletedTask;
        foreach (var player in participants.OfType<Player>())
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

    public static int GetNewTide(Player player, int tideChange, out int numOverflowed, bool negative = false)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        var tide = tideCombatState.CurrentTide;
        numOverflowed = negative ? 0 : Mathf.FloorToInt((tideChange + tide) / tideCombatState.MaxTide);
        return negative ? Math.Max(tide - tideChange, 0) : Mathf.PosMod(tideChange + tide, tideCombatState.MaxTide);
    }

    public static async Task UpdateTide(Player player, int tideChange, bool negative = false)
    {
        var shouldGainTide =
            PercyJacksonHooks.ShouldGainTide(player.Creature.CombatState, new ThrowingPlayerChoiceContext(), player);
        if (!negative && shouldGainTide) await RaiseTide(player, tideChange);
        else if (negative) await LowerTide(player, tideChange);
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

    private static async Task RaiseTide(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        if (tideCombatState.CurrentTide + tideChange > tideCombatState.MaxTide)
            await IncreaseMaxTideFromOverflow(player, tideChange);
        else tideCombatState.CurrentTide += tideChange;
        tideCombatState.TideGainedThisCombat += tideChange;
    }

    private static async Task LowerTide(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        if (tideCombatState.CurrentTide - tideChange > 0) player.PlayerCombatState.Tide().CurrentTide -= tideChange;
        await Task.CompletedTask;
    }

    private static async Task IncreaseMaxTideFromOverflow(Player player, int tideChange)
    {
        var tideCombatState = player.PlayerCombatState.Tide();
        var previousTide = tideCombatState.CurrentTide;
        var maxTide = tideCombatState.MaxTide;
        var tideLeft = previousTide + tideChange;

        while (tideLeft > maxTide) // hoo boy
        {
            tideLeft -= maxTide;
            tideCombatState.MaxTide += 1;
            await PercyJacksonHooks.OnTideOverflowed(player.Creature.CombatState, new ThrowingPlayerChoiceContext(),
                player);
            maxTide = tideCombatState.MaxTide;
        }

        tideCombatState.CurrentTide = tideLeft;
    }
}