using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Stormbringer: PercyJacksonCard
{
    public Stormbringer() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(TideKeyword);
        WithVar("PlusDamage", 0, 1);
    }
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await TideManager.UpdateTide(Owner, ResolveEnergyXValue() + DynamicVars["PlusDamage"].IntValue);
    }
}