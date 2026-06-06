using System.Diagnostics.CodeAnalysis;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Fields;

namespace PercyJackson.PercyJacksonCode.Extensions;

public static class PlayerCombatStateExtensions
{
    public class TideCombatState(PlayerCombatState playerCombatState)
    {
        public int CurrentTide         {
            get;
            set
            {
                if (field == value) return;
                var tide = field;
                field = value;
                TideChanged?.Invoke(tide, field);
            }
        } = 0;
        
        public int MaxTide         {
            get => TempMaxTide > 0 ? TempMaxTide : field;
            set
            {
                if (field == value) return;
                var maxTide = field;
                field = value;
                MaxTideChanged?.Invoke(maxTide, field);
            }
        } = 6;
        
        public int TempMaxTide         {
            get;
            set
            {
                if (field == value) return;
                var maxTide = field;
                field = value;
                MaxTideChanged?.Invoke(maxTide, field);
            }
        } = 0;

        public Player Owner
        {
            get;
            set
            {
                if (field != null && value != null && value != field)
                    throw new InvalidOperationException("Tide already has an owner.");

                field = value;
            }
        }

        public event Action<int, int>? TideChanged;
        public event Action<int, int>? MaxTideChanged;
    }
    
    public static TideCombatState Tide(this PlayerCombatState playerCombatState)
    {
        return PercyJacksonFields.TideCombatState[playerCombatState];
    }
}