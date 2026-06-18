using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class TerrifiedAnticipation : PercyJacksonCard
{
    public TerrifiedAnticipation() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(ComboKeyword);
        WithVar("DamageAmount", 2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<TerrifiedAnticipationPower>(choiceContext, Owner.Creature,
            DynamicVars["DamageAmount"].BaseValue, Owner.Creature, this);
    }
}