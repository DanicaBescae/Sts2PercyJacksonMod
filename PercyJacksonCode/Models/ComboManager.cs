using System.Buffers;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Uncommon;
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
    private CardModel? _cardToIncrease;
    private decimal _multPerCombo = 10M;
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (!WillIncreaseCombo(cardPlay.Card))
            return Task.CompletedTask;
        
        var playerCombo = cardPlay.Card.Owner.PlayerCombatState.Combo();

        if (playerCombo.CurrentComboCount > 0)
        {
            _cardToIncrease = cardPlay.Card;
        }
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (!props.IsPoweredAttack() || cardSource == null ||
            dealer != cardSource.Owner.Creature && dealer != cardSource.Owner.Osty || dealer == null)
            return 1M;

        var combo = cardSource.Owner.PlayerCombatState.Combo();

        if (combo == null) return 1M;

        if (_cardToIncrease != null)
        {
            return cardSource == _cardToIncrease
                ? (_cardToIncrease is TrueFinisher
                    ? CurrentComboMult(_cardToIncrease.Owner, 2, true)
                    : CurrentComboMult(_cardToIncrease.Owner))
                : 1M;
        }

        var pile = cardSource.Pile;
        if (pile == null || pile.Type == PileType.Play) return 1M;
        return combo.CurrentComboCount > 0 && WillIncreaseCombo(cardSource)
            ? (cardSource is TrueFinisher
                ? CurrentComboMult(_cardToIncrease.Owner, 2, true)
                : CurrentComboMult(_cardToIncrease.Owner))
            : 1M;
    }

    public override decimal ModifyBlockMultiplicative(
        Creature target,
        decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (props != ValueProp.Move || cardSource == null)
            return 1M;

        var combo = cardSource.Owner.PlayerCombatState.Combo();

        if (combo == null) return 1M;

        if (_cardToIncrease != null)
        {
            return cardSource == _cardToIncrease
                ? (_cardToIncrease is TrueFinisher
                    ? CurrentComboMult(_cardToIncrease.Owner, 2, true)
                    : CurrentComboMult(_cardToIncrease.Owner))
                : 1M;
        }

        var pile = cardSource.Pile;
        if (pile == null || pile.Type == PileType.Play) return 1M;
        return combo.CurrentComboCount > 0 && WillIncreaseCombo(cardSource)
            ? (cardSource is TrueFinisher
                ? CurrentComboMult(_cardToIncrease.Owner, 2, true)
                : CurrentComboMult(_cardToIncrease.Owner))
            : 1M;
    }

    public decimal CurrentComboMult(Player player, decimal multMod = 1, bool multiplicativeMult = false)
    {
        if (multMod > 0 && multiplicativeMult)
            return 1 + ((_multPerCombo * multMod * player.PlayerCombatState.Combo().CurrentComboCount) / 100);
        if (multMod > 0 && !multiplicativeMult)
            return 1 + (((_multPerCombo + multMod) * player.PlayerCombatState.Combo().CurrentComboCount) / 100);
        return 1 + ((_multPerCombo * player.PlayerCombatState.Combo().CurrentComboCount) / 100);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatManager.Instance.IsOverOrEnding || cardPlay.Card.Owner.Creature.CombatState == null)
            return;

        var combatState = cardPlay.Card.Owner.Creature.CombatState;
        var playerCombo = cardPlay.Card.Owner.PlayerCombatState.Combo();

        // If we have the improvising power, we can increase our combo no matter what
        if (cardPlay.Card.Owner.HasPower<ImprovisingPower>())
        {
            await UpdateCombo(choiceContext, combatState, cardPlay, playerCombo);
        }
        // End combo if this card can't increase combo
        else if (!WillIncreaseCombo(cardPlay.Card))
        {
            await ClearCombo(choiceContext, combatState, playerCombo);
        }
        // We can increase the combo if we're starting a combo or if we already started one
        else if (StartsCombo(cardPlay.Card, playerCombo.CurrentComboCount) || playerCombo.CurrentComboCount > 0)
        {
            await UpdateCombo(choiceContext, combatState, cardPlay, playerCombo);
        }

        if (_cardToIncrease != null && cardPlay.Card == _cardToIncrease) _cardToIncrease = null;
    }
    
    /*
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (CombatManager.Instance.IsOverOrEnding || side != CombatSide.Player) return;
        
        foreach (var player in participants.Where(c => c.IsPlayer))
        {
            var combatState = player.CombatState;
            var combo = player.Player.PlayerCombatState.Combo();
            await ClearCombo(choiceContext, combatState, combo);
        }
    }*/

    public static bool IsComboChainCard(CardModel card)
    {
        return card.DynamicVars.ContainsKey("ComboX");
    }

    public static bool IsComboCard(CardModel card)
    {
        return IsComboChainCard(card) ||
               card.Keywords.Contains(PercyJacksonCard.ComboStarter) ||
               card.Owner.HasPower<ImprovisingPower>();
    }

    public static bool WillIncreaseCombo(CardModel card)
    {
        return card.Type == CardType.Attack || IsComboChainCard(card) ||
               card.Keywords.Contains(PercyJacksonCard.ComboStarter) ||
               card.Owner.HasPower<ImprovisingPower>();
    }

    private static bool StartsCombo(CardModel card, int currentCombo)
    {
        return (card.Keywords.Contains(PercyJacksonCard.ComboStarter) || IsComboChainCard(card)) && currentCombo == 0;
    }

    private static async Task UpdateCombo(PlayerChoiceContext choiceContext, ICombatState combatState,
        CardPlay cardPlay, PlayerCombatStateExtensions.ComboCombatState combo)
    {
        if (combo.CurrentComboCount == 0)
            await PercyJacksonHooks.AfterComboStarted(combatState, choiceContext, cardPlay.Card);
        combo.AddToCombo(cardPlay);
    }

    public static async Task ClearCombo(PlayerChoiceContext choiceContext, ICombatState combatState,
        PlayerCombatStateExtensions.ComboCombatState combo)
    {
        if (combo.ComboHistory.Count > 0)
            await PercyJacksonHooks.AfterComboEnded(combatState, choiceContext, combo.Owner, combo.ComboHistory.Count);
        combo.ClearCombo();
    }
}