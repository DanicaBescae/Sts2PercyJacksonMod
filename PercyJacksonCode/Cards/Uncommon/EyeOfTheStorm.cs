using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class EyeOfTheStorm: PercyJacksonCard
{
    private bool _needApplyNoTide;
    
    public EyeOfTheStorm() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("BlockAmt", 5, 2);
        WithCalculatedBlock(0,
            (c, _) => c.Owner.PlayerCombatState.Tide().CurrentTide * c.DynamicVars["BlockAmt"].IntValue);
        WithKeyword(TideKeyword);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.CalculatedBlock, cardPlay);
        await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.CalculatedBlock.IntValue,
            Owner.Creature, this);
        _needApplyNoTide = true;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!_needApplyNoTide || Owner != player) return;
        await PowerCmd.Apply<NoTideGainPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        _needApplyNoTide = false;
    }
}