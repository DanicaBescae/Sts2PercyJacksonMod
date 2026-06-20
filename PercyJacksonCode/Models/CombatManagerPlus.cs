using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace PercyJackson.PercyJacksonCode.Models;

public class CombatManagerPlus(): CustomSingletonModel(HookType.Combat)
{
    private static Dictionary<Player, int> timesShuffledThisCombat = new();
    private static Dictionary<Player, int> cardsDrawnThisTurn = new();

    public static int GetTimesShuffledThisCombat(Player player)
    {
        return timesShuffledThisCombat.GetValueOrDefault(player, 0);
    }
    
    public static int GetCardsDrawnThisTurn(Player player)
    {
        return cardsDrawnThisTurn.GetValueOrDefault(player, 0);
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (fromHandDraw) return Task.CompletedTask;
        if (!cardsDrawnThisTurn.TryAdd(card.Owner, 1)) cardsDrawnThisTurn[card.Owner]++;
        return Task.CompletedTask;
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