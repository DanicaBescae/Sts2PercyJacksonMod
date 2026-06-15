using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class MiniOffering: PercyJacksonCard
{
    public MiniOffering() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithEnergy(2);
        WithCards(3, 1);
        WithTide(2);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue, true);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CommonActions.Draw(this, choiceContext);
    }
}