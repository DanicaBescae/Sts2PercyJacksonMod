using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Models;

/// <summary>
/// Manages the combo counter throughout combat. Keeps track of which cards were played in the current combo in order
/// and what the current combo count is. Also resets combos on turn end or non-Combo card played.
/// </summary>
public class ComboManager(): CustomSingletonModel(HookType.Combat)
{
    public static int CurrentComboCount { get; private set; }
    
    public static List<CardPlay> ComboHistory = [];
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatManager.Instance.IsOverOrEnding || cardPlay.Card.Owner.Creature.CombatState == null)
            return Task.CompletedTask;
        
        if (cardPlay.Card.Type == CardType.Attack || cardPlay.Card.Tags.Contains<CardTag>(PercyJacksonCard.ComboTag))
        {
            UpdateCombo(cardPlay);
        }
        else
        {
            ClearCombo();
        }
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (CombatManager.Instance.IsOverOrEnding || side != CombatSide.Player)
            return Task.CompletedTask;
        
        ClearCombo();
        return Task.CompletedTask;
    }

    private static void UpdateCombo(CardPlay cardPlay)
    {
        ComboHistory.Add(cardPlay);
        CurrentComboCount = ComboHistory.Count;
    }
    
    private static void ClearCombo()
    {
        ComboHistory.Clear();
        CurrentComboCount = 0;
    }
}