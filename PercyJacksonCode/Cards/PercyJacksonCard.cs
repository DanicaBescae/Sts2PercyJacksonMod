using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using HarmonyLib;
using PercyJackson.PercyJacksonCode.Character;
using PercyJackson.PercyJacksonCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.DynamicVars;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards;

[Pool(typeof(PercyJacksonCardPool))]
public abstract class PercyJacksonCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    [CustomEnum] 
    public static CardTag ComboTag;

    private int ComboNeeded { get; set; } = 0;
    private bool NeedComboToPlay { get; set; } = false;
    
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

    /// <summary>
    /// Generates a DynamicVar with base values and sets up this card for Combos.
    /// </summary>
    /// <param name="baseVal">Combo needed to unlock effects pre-upgrade.</param>
    /// <param name="upgrade">Combo needed to unlock effects post-upgrade.</param>
    /// <returns></returns>
    protected PercyJacksonCard WithCombo(int baseVal, int upgrade = 0)
    {
        WithVar(new ComboVar(baseVal).WithUpgrade(upgrade));
        WithTags(ComboTag);
        this.ComboNeeded = this.IsUpgraded ? upgrade : baseVal;
        return this;
    }

    /// <summary>
    /// Check if the minimum Combo count has been reached for this card.
    /// </summary>
    /// <returns></returns>
    protected static bool IsComboComplete(CardModel card)
    {
        if (card is not PercyJacksonCard thisCard) return true;
        if (!thisCard.Tags.Contains(ComboTag) || thisCard.ComboNeeded <= 0) return true;
        return ComboManager.CurrentComboCount >= thisCard.ComboNeeded;
    }

    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is not PercyJacksonCard thisCard) return true;
        // Check for Combo count
        return IsComboComplete(thisCard) || !thisCard.NeedComboToPlay;
    }
}