using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Stormbringer: PercyJacksonCard
{
    public Stormbringer() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(TideKeyword);
        WithTip(typeof(WoundedPower));
        WithVar("PlusDamage", 0, 1);
    }
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var energy = ResolveEnergyXValue();
        await PowerCmd.Apply<WoundedPower>(choiceContext, cardPlay.Target, energy + DynamicVars["PlusDamage"].IntValue,
            Owner.Creature, this);
        await TideManager.UpdateTide(Owner, energy + DynamicVars["PlusDamage"].IntValue);
    }
}