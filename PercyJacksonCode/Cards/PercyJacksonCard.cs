using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Character;
using PercyJackson.PercyJacksonCode.DynamicVars;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards;

[Pool(typeof(PercyJacksonCardPool))]
public abstract class PercyJacksonCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    [CustomEnum]
    public static CardKeyword TideKeyword;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword ComboStarter;
    
    [CustomEnum]
    public static CardKeyword ComboKeyword;

    private int ComboNeeded { get; set; }
    private bool NeedComboToPlay { get; set; }
    
    private readonly List<TemporaryCardCost> _temporaryCombos = [];
    public TemporaryCardCost? TemporaryCombo => _temporaryCombos.LastOrDefault();
    
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    public void SetComboThisTurn(int combo)
    {
        AddTemporaryCombo(TemporaryCardCost.ThisTurn(combo));
    }

    private void AddTemporaryCombo(TemporaryCardCost combo)
    {
        AssertMutable();
        _temporaryCombos.Add(combo);
        // ComboChanged?.Invoke();
    }

    public int RemoveEndOfTurnCombo()
    {
        return _temporaryCombos.RemoveAll(c => c.ClearsWhenTurnEnds);
    }

    protected PercyJacksonCard WithComboStarter()
    {
        WithKeyword(ComboStarter);
        WithKeyword(ComboKeyword);
        return this;
    }

    /// <summary>
    /// Generates a DynamicVar with base values, adds tooltip, and sets up this card for Combos.
    /// </summary>
    /// <param name="baseVal">Combo needed to unlock effects pre-upgrade.</param>
    /// <param name="upgrade">Combo needed to unlock effects post-upgrade.</param>
    /// <param name="needCombo">Does this card need the combo to be played at all?</param>
    /// <returns></returns>
    protected PercyJacksonCard WithCombo(int baseVal, int upgrade = 0, bool needCombo=false)
    {
        WithVar(new ComboVar(baseVal).WithUpgrade(upgrade));
        ComboNeeded = IsUpgraded ? upgrade : baseVal;
        //MainFile.Logger.Info("Combo needed in WithCombo function: " + ComboNeeded + " for card: " + this);
        NeedComboToPlay = needCombo;
        return this;
    }
    
    /// <summary>
    /// Adds Tide keyword to card and generates a DynamicVar with base values.
    /// </summary>
    /// <returns></returns>
    protected PercyJacksonCard WithTide(int baseVal, int upgrade = 0)
    {
        WithVar(new DynamicVar("Tide", baseVal).WithUpgrade(upgrade));
        WithKeyword(TideKeyword);
        return this;
    }

    /// <summary>
    /// Check if the minimum Combo count has been reached for this card.
    /// </summary>
    /// <returns></returns>
    protected static bool IsComboComplete(CardModel card)
    {
        if (card.Owner.HasPower<ImprovisingPower>()) return true;
        if (card is not PercyJacksonCard thisCard) return true;
        var comboNeededThisTurn =
            thisCard._temporaryCombos.Count > 0 ? thisCard._temporaryCombos.Min().Cost : thisCard.ComboNeeded;
        //MainFile.Logger.Info("Combo needed in IsComboComplete function: " + comboNeededThisTurn + " for card: " + card);
        if (!ComboManager.IsComboChainCard(card) || comboNeededThisTurn <= 0) return true;
        return card.Owner.PlayerCombatState.Combo().CurrentComboCount >= comboNeededThisTurn;
    }

    protected override bool ShouldGlowGoldInternal => ComboManager.IsComboChainCard(this) && IsComboComplete(this);

    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card.Owner.HasPower<ImprovisingPower>()) return true;
        if (card.Keywords.Contains(ComboStarter)) return card.Owner.PlayerCombatState.Combo().CurrentComboCount == 0;
        if (!ComboManager.IsComboChainCard(card)) return true;
        
        if (card is PercyJacksonCard pjoCard) return IsComboComplete(pjoCard) || !pjoCard.NeedComboToPlay;
        return IsComboComplete(card);
    }
}