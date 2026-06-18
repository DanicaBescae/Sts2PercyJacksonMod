using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class PureDestruction : PercyJacksonCard
{
    public PureDestruction() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("DamageBoost", 2, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<PureDestructionPower>(choiceContext, Owner.Creature, this,
            DynamicVars["DamageBoost"].BaseValue);
    }
}