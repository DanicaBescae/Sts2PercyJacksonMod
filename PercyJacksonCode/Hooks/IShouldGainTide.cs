using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IShouldGainTide
{
    public bool ShouldGainTide(PlayerChoiceContext choiceContext, Player player);
}