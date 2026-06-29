using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Stormbringer: PercyJacksonCard
{
    public Stormbringer() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(TideKeyword);
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var energy = ResolveEnergyXValue();
        await TideManager.UpdateMaxTide(Owner, energy + (IsUpgraded ? 0 : 1));
        await TideManager.UpdateTide(Owner,
            Owner.PlayerCombatState.Tide().MaxTide - Owner.PlayerCombatState.Tide().CurrentTide);
    }
}