using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class AchillesHeel: PercyJacksonCard
{
    public AchillesHeel() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithEnergy(2);
        WithPower<DexterityPower>(3);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars["DexterityPower"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<AchillesHeelPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}