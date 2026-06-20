using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class HurricaneForm : PercyJacksonCard
{
    public HurricaneForm() : base(3, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithVar("DamageAmount", 2, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HurricaneFormPower>(choiceContext, Owner.Creature, DynamicVars["DamageAmount"].BaseValue,
            Owner.Creature, this);
    }
}