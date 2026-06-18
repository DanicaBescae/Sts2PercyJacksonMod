using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IAfterWaterActivated
{
    public Task AfterWaterActivated(PlayerChoiceContext choiceContext, CardModel card);
}