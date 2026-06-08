namespace PercyJackson.PercyJacksonCode.Hooks;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

public static class PercyJacksonHooks
{
    private static async Task Dispatch<T>(ICombatState combatState, PlayerChoiceContext choiceContext,
        Func<T, Task> action) where T : class
    {
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = (AbstractModel)(object)model;
            choiceContext.PushModel(abstractModel);
            await action(model);
            abstractModel.InvokeExecutionFinished();
            choiceContext.PopModel(abstractModel);
        }
    }
    
    public static Task AfterComboEnded(ICombatState combatState, PlayerChoiceContext choiceContext, int combo)
    {
        return Dispatch<IAfterComboEnded>(combatState, choiceContext,
            model => model.AfterComboEnded(choiceContext, combo));
    }
    
    public static Task AfterComboStarted(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card)
    {
        return Dispatch<IAfterComboStarted>(combatState, choiceContext,
            model => model.AfterComboStarted(choiceContext, card));
    }
    
    public static Task OnTideOverflowed(ICombatState combatState, PlayerChoiceContext choiceContext, Player player)
    {
        return Dispatch<IOnTideOverflowed>(combatState, choiceContext,
            model => model.OnTideOverflowed(choiceContext, player));
    }
    
    public static bool ShouldGainTide(ICombatState combatState, PlayerChoiceContext choiceContext, Player player)
    {
        foreach (var model in combatState.IterateHookListeners().OfType<IShouldGainTide>())
        {
            if (!model.ShouldGainTide(choiceContext, player)) return false;
        }

        return true;
    }
}