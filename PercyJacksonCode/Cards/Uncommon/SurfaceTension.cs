using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SurfaceTension: PercyJacksonCard
{
    public SurfaceTension() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithTide(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SurfaceTensionPower>(choiceContext, Owner.Creature, DynamicVars["WoundedPower"].BaseValue,
            Owner.Creature, this);
    }
}