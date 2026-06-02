using MegaCrit.Sts2.Core.Entities.Players;
using PercyJackson.PercyJacksonCode.Fields;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Extensions;

public static class PlayerCombatStateExtensions
{
    public static TideCombatState Tide(this PlayerCombatState playerCombatState)
    {
        return PercyJacksonFields.TideCombatState[playerCombatState];
    }
}