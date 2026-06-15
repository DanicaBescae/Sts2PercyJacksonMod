using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class SwordSharpening: PercyJacksonCard
{
    private const int BaseDamage = 1;

    [SavedProperty]
    public int CurrentDamage
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            DynamicVars["SharpeningAmount"].BaseValue = field;
        }
    } = 1;

    [SavedProperty]
    private int IncreasedDamage
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }

    public SwordSharpening() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("SharpeningAmount", 1);
        WithVar("Increase", 1);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithComboStarter();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<SwordSharpeningPower>(choiceContext, Owner.Creature,
            DynamicVars["SharpeningAmount"].BaseValue, Owner.Creature, this);
        var intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        if (DeckVersion is not SwordSharpening deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
    }
    
    protected override void AfterDowngraded() => UpdateDamage();

    private void BuffFromPlay(int extraDamage)
    {
        IncreasedDamage += extraDamage;
        UpdateDamage();
    }

    private void UpdateDamage() => CurrentDamage = BaseDamage + IncreasedDamage;
}