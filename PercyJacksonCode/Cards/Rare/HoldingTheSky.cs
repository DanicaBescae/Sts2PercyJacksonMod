using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class HoldingTheSky : PercyJacksonCard
{
    public HoldingTheSky() : base(1, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithVar("ExtraBlock", 2);
        WithCalculatedBlock(5,
            (c, _) => PileType.Draw.GetPile(c.Owner).Cards.Count * c.DynamicVars["ExtraBlock"].BaseValue);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }
}