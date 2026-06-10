using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Powers;

public class GuiltyPower: PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    private int GetNumCreaturesTormented()
    {
        return CombatState.HittableEnemies.Where( c =>
        {
            if (c.GetPower<TormentedPower>() == null) return false;
            return c.GetPower<TormentedPower>().tormentor == Owner;
        }).Count();
    }
    
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Side) return;

        if (Owner.Player is null) return;
        
        var creaturesTormenting = GetNumCreaturesTormented();
        
        var guiltyCards = new CardPileAddResult[creaturesTormenting];

        for (var i = 0; i < creaturesTormenting; i++)
        {
            var guiltyCard = CombatState.CreateCard<Guilty>(Owner.Player);
            guiltyCard.AddKeyword(CardKeyword.Ethereal);
            guiltyCards[i] =
                await CardPileCmd.AddGeneratedCardToCombat(guiltyCard, PileType.Draw, Owner.Player,
                    CardPilePosition.Random);
        }
        CardCmd.PreviewCardPileAdd(guiltyCards);
        await Cmd.Wait(0.2f);
    }
    
    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (!creature.HasPower<TormentedPower>()) return;
        if (GetNumCreaturesTormented() == 0) await PowerCmd.Remove(this);
    }
}