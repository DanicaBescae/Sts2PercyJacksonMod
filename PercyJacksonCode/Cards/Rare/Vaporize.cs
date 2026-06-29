using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Vaporize : PercyJacksonCard
{
    public Vaporize() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(18, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var results = (await CommonActions.CardAttack(this, play).Execute(choiceContext)).Results;
        foreach (var resultList in results)
        {
            foreach (var result in resultList.Where(result =>
                         result is { WasTargetKilled: false, Receiver.IsDead: false } &&
                         result.Receiver.CurrentHp < result.Receiver.MaxHp / 10))
            {
                await CreatureCmd.Kill(result.Receiver);
            }
        }
    }
}