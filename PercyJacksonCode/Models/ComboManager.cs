using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Hooks;

namespace PercyJackson.PercyJacksonCode.Models;

/// <summary>
/// Manages the combo counter throughout combat. Keeps track of which cards were played in the current combo in order
/// and what the current combo count is. Also resets combos on turn end or non-Combo card played.
/// </summary>
public class ComboManager(): CustomSingletonModel(HookType.Combat)
{
    public static int CurrentComboCount { get; private set; }

    private static readonly List<CardPlay> ComboHistory = [];

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatManager.Instance.IsOverOrEnding || cardPlay.Card.Owner.Creature.CombatState == null)
            return Task.CompletedTask;
        
        var combatState = cardPlay.Card.Owner.Creature.CombatState;
        
        // End combo if this is not an attack or combo card
        if (cardPlay.Card.Type != CardType.Attack && !cardPlay.Card.Tags.Contains(PercyJacksonCard.ComboTag))
        {
            return ClearCombo(choiceContext, combatState);
        }
        
        // If we haven't started a combo, and this isn't a combo starter, we can't continue a combo
        if (!cardPlay.Card.Keywords.Contains(PercyJacksonCard.ComboStarter) && CurrentComboCount == 0)
            return Task.CompletedTask;

        return UpdateCombo(choiceContext, combatState, cardPlay);
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (CombatManager.Instance.IsOverOrEnding || side != CombatSide.Player)
            return Task.CompletedTask;

        var combatState = participants.FirstOrDefault()?.CombatState;
        return ClearCombo(choiceContext, combatState);
    }

    private static async Task UpdateCombo(PlayerChoiceContext choiceContext, ICombatState combatState, CardPlay cardPlay)
    {
        await PercyJacksonHooks.AfterComboStarted(combatState, choiceContext, cardPlay.Card);
        ComboHistory.Add(cardPlay);
        CurrentComboCount = ComboHistory.Count;
    }
    
    private static async Task ClearCombo(PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (ComboHistory.Count > 0) await PercyJacksonHooks.AfterComboEnded(combatState, choiceContext, ComboHistory.Count);
        ComboHistory.Clear();
        CurrentComboCount = 0;
    }
}