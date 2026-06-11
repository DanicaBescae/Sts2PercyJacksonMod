using System.Buffers;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Fields;
using PercyJackson.PercyJacksonCode.Hooks;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Models;

/// <summary>
/// Manages the combo counter throughout combat. Keeps track of which cards were played in the current combo in order
/// and what the current combo count is. Also resets combos on turn end or non-Combo card played.
/// </summary>
public class ComboManager(): CustomSingletonModel(HookType.Combat)
{
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatManager.Instance.IsOverOrEnding || cardPlay.Card.Owner.Creature.CombatState == null)
            return Task.CompletedTask;

        var combatState = cardPlay.Card.Owner.Creature.CombatState;
        var playerCombo = cardPlay.Card.Owner.PlayerCombatState.Combo();
        
        // If we have the improvising power, we can increase our combo no matter what
        if (cardPlay.Card.Owner.HasPower<ImprovisingPower>()) return UpdateCombo(choiceContext, combatState, cardPlay, playerCombo);

        // End combo if this is not an attack or combo card
        if (cardPlay.Card.Type != CardType.Attack && !IsComboCard(cardPlay.Card))
        {
            return ClearCombo(choiceContext, combatState, playerCombo);
        }

        // If we haven't started a combo, and this isn't a combo starter, we can't continue a combo
        if (!cardPlay.Card.Keywords.Contains(PercyJacksonCard.ComboStarter) && playerCombo.CurrentComboCount == 0)
            return Task.CompletedTask;

        return UpdateCombo(choiceContext, combatState, cardPlay, playerCombo);
    }

    public static bool IsComboChainCard(CardModel card)
    {
        return card.DynamicVars.ContainsKey("ComboX");
    }

    public static bool IsComboCard(CardModel card)
    {
        return IsComboChainCard(card) || card.Keywords.Contains(PercyJacksonCard.ComboStarter) ||
               card.Owner.HasPower<ImprovisingPower>();
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (CombatManager.Instance.IsOverOrEnding || side != CombatSide.Player) return;
        
        var players = participants.OfType<Player>();
        foreach (var player in players)
        {
            var combatState = player.Creature.CombatState;
            var combo = player.PlayerCombatState.Combo();
            await ClearCombo(choiceContext, combatState, combo);
        }
    }

    private static async Task UpdateCombo(PlayerChoiceContext choiceContext, ICombatState combatState,
        CardPlay cardPlay, PlayerCombatStateExtensions.ComboCombatState combo)
    {
        await PercyJacksonHooks.AfterComboStarted(combatState, choiceContext, cardPlay.Card);
        combo.AddToCombo(cardPlay);
    }

    public static async Task ClearCombo(PlayerChoiceContext choiceContext, ICombatState combatState,
        PlayerCombatStateExtensions.ComboCombatState combo)
    {
        if (combo.ComboHistory.Count > 0)
            await PercyJacksonHooks.AfterComboEnded(combatState, choiceContext, combo.ComboHistory.Count);
        combo.ClearCombo();
    }
}