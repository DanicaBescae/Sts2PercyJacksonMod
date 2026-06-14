using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Patches;

namespace PercyJackson.PercyJacksonCode.Nodes;

public partial class NComboDisplay: Control
{
    private MegaLabel? _label;
    // text, shadow, outline
    private static readonly (Color, Color, Color) RedColor = (new Color("4D1311"), new Color("301C1B"),
        new Color("0F0B0B"));
    
    private int _displayedCombo = -1;
    private float _lerpingComboCount;
    private float _velocity1;
    
    private Player? _player;
    
    private bool _isListeningToCombatState;
    
    private HoverTip _hoverTip;
    
    public void Initialize(Player player)
    {
        _player = player;
        ConnectComboChangedSignal();
        RefreshVisibility();
    }
    
    public override void _Ready()
    {
        _label = CreateLabel(RedColor);
        GetNode<MarginContainer>("%TextContainer").AddChild(_label);
		
        var locString = new LocString("card_keywords", "PERCYJACKSON-COMBOKEYWORD.description");
        _hoverTip = new HoverTip(new LocString("card_keywords", "PERCYJACKSON-COMBOKEYWORD.title"), locString);
		
        Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
        Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
		
        SetComboText(0, true);
        Visible = true;
    }
    
    public override void _Process(double delta)
    {
        if (_player == null) return;
        var combo = GetPlayerComboCount(_player);

        _lerpingComboCount =
            MathHelper.SmoothDamp(_lerpingComboCount, combo, ref _velocity1, 0.5f, (float)delta);
        SetComboText((int)MathF.Round(_lerpingComboCount));
    }
    
    private static MegaLabel CreateLabel((Color, Color, Color) fontColor)
    {
        var label = new MegaLabel();
        label.MaxFontSize = 28;
        label.AutoSizeEnabled = false;
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.VerticalAlignment = VerticalAlignment.Center;
        label.AddThemeColorOverride("font_color", fontColor.Item1);
        label.AddThemeColorOverride("font_shadow_color", fontColor.Item2);
        label.AddThemeColorOverride("font_outline_color", fontColor.Item3);
        label.AddThemeFontOverride("font", new SystemFont());
        label.AddThemeConstantOverride("shadow_offset_x", 2);
        label.AddThemeConstantOverride("shadow_offset_y", 2);
        label.AddThemeConstantOverride("outline_size", 10);
        label.AddThemeConstantOverride("shadow_outline_size", 10);
        label.AddThemeFontSizeOverride("font_size", 20);
        label.Text = "1 COMBO";

        return label;
    }
    
    public override void _EnterTree()
    {
        base._EnterTree();
        ConnectComboChangedSignal();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (_player == null || !_isListeningToCombatState) return;
		
        var combo = _player.PlayerCombatState?.Combo();
        combo?.ComboChanged -= OnComboChanged;
        _isListeningToCombatState = false;
    }
    
    private void OnHovered()
    {
        var nHoverTipSet = NHoverTipSet.CreateAndShow(this, _hoverTip);
        nHoverTipSet?.GlobalPosition = GlobalPosition + new Vector2(-70, -240);
    }
	
    private void OnUnhovered()
    {
        NHoverTipSet.Remove(this);
    }
	
    private void ConnectComboChangedSignal()
    {
        if (_player == null || _isListeningToCombatState) return;
		
        var combo = _player.PlayerCombatState?.Combo();
        combo?.ComboChanged += OnComboChanged;
        _isListeningToCombatState = true;
    }
    
    private void OnComboChanged(int oldCombo, int newCombo)
    {
        UpdateComboCount(oldCombo, newCombo);
        RefreshVisibility();
    }
    
    private static int GetPlayerComboCount(Player player)
    {
        var tide = player.PlayerCombatState?.Combo().CurrentComboCount ?? 0;
        return tide;
    }
    
    private void UpdateComboCount(int oldCombo, int newCombo)
    {
        if (newCombo >= oldCombo) return;
        
        _lerpingComboCount = newCombo;
        SetComboText(newCombo);
    }
    
    private void SetComboText(int combo, bool initSetup = false)
    {
        if (!initSetup && _displayedCombo == combo) return;
		
        _displayedCombo = combo;
        var label = _label;
        label.SetTextAutoSize(combo + "COMBO");
    }
    
    private void RefreshVisibility()
    {
        if (_player == null)
        {
            Visible = false;
            return;
        }

        Visible = GetPlayerComboCount(_player) > 0;
    }
}