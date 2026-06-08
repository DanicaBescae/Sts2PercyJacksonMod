using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class PoseidonsBlessing: PercyJacksonCard
{
    public PoseidonsBlessing() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithTip(typeof(VigorPower));
        WithVar("Times", 1, 1);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<PoseidonsBlessingPower>(choiceContext, Owner.Creature, DynamicVars["Times"].BaseValue,
            Owner.Creature,
            this);
    }
}