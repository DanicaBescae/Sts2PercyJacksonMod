using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class WavePunch: PercyJacksonCard
{
    public WavePunch() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithTide(0, 3);
        WithPower<WavePunchPower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsUpgraded) await TideManager.UpdateTide(Owner, 3);
        await PowerCmd.Apply<WavePunchPower>(choiceContext, Owner.Creature, DynamicVars["WavePunchPower"].BaseValue,
            Owner.Creature, this);
    }
}