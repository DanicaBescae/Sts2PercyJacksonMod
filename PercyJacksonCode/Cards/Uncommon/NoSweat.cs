using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class NoSweat : PercyJacksonCard
{
    public NoSweat() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergy(1);
        WithKeyword(ComboStarter, UpgradeType.Add);
        WithKeyword(ComboKeyword);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<NoSweatPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature,
            this);
    }
}