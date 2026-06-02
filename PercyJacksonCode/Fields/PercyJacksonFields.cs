using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Fields;

public class PercyJacksonFields
{
    public static readonly SpireField<PlayerCombatState, TideCombatState> TideCombatState = new(()=>null);
}