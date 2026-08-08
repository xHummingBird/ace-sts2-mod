using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Ace.AceCode.Mechanics;

public partial class CardDisplayOverlay : Control
{
    public static CardDisplayOverlay? Instance { get; private set; }

    private Control? _cardDisplay;

    private TextureRect? _orb;

    private TextureRect? _card1;
    private TextureRect? _card2;
    private TextureRect? _card3;
    private TextureRect? _card4;

    private RichTextLabel? _orbLabel;

    private Player? _player;

    private bool _exiting;
    
    

    public override void _Ready()
    {
        Instance = this;
        Name = "CardDisplayOverlay";

        MouseFilter = MouseFilterEnum.Pass;

        CallDeferred(nameof(Setup));
    }

    private async void Setup()
    {
        if (!IsInsideTree())
            return;

        for (int i = 0; i < 60; i++)
        {
            if (_exiting || !IsInsideTree())
                return;

            var state =
                CombatManager.Instance?.DebugOnlyGetState();

            var player =
                state?.Players.FirstOrDefault(
                    p => LocalContext.IsMe(p)
                );

            if (player != null)
            {
                if (player.Character is not Character.Ace)
                {
                    QueueFree();
                    return;
                }

                _player = player;
                break;
            }

            await ToSignal(
                GetTree(),
                SceneTree.SignalName.ProcessFrame
            );
            
            
        }

        if (_player == null)
        {
            QueueFree();
            return;
        }

        var scene = GD.Load<PackedScene>(
            "res://Ace/scenes/card_holder_display.tscn"
        );

        if (scene == null)
        {
            GD.PushError(
                "[Ace] Failed to load card_holder_display.tscn"
            );

            QueueFree();
            return;
        }

        _cardDisplay = scene.Instantiate<Control>();

        AddChild(_cardDisplay);

        _cardDisplay.MouseFilter =
            MouseFilterEnum.Pass;

        _cardDisplay.SetAnchorsPreset(
            LayoutPreset.BottomLeft
        );

        //
        // Tweak as needed
        //
        _cardDisplay.Position =
            new Vector2(-135, -100);

        _orb =
            _cardDisplay.GetNodeOrNull<TextureRect>(
                "%Orb"
            );

        _card1 =
            _cardDisplay.GetNodeOrNull<TextureRect>(
                "%Card1"
            );

        _card2 =
            _cardDisplay.GetNodeOrNull<TextureRect>(
                "%Card2"
            );

        _card3 =
            _cardDisplay.GetNodeOrNull<TextureRect>(
                "%Card3"
            );

        _card4 =
            _cardDisplay.GetNodeOrNull<TextureRect>(
                "%Card4"
            );

        _orbLabel =
            _cardDisplay.GetNodeOrNull<RichTextLabel>(
                "%OrbLabel"
            );

        if (_orbLabel != null)
        {
            SetupLabel(_orbLabel);
            _orbLabel.Position += new Vector2(0, 17);
        }

        RefreshDisplay();
    }

    public override void _Process(double delta)
    {
        if (_exiting)
            return;

        if (_player == null)
            return;

        if (!CombatManager.Instance.IsInProgress)
            return;

        RefreshDisplay();
    }

    private void SetupLabel(
        RichTextLabel label)
    {
        var font =
            GD.Load<Font>(
                "res://themes/kreon_bold_shared.tres"
            );

        if (font != null)
        {
            label.AddThemeFontOverride(
                "font",
                font
            );

            label.AddThemeFontOverride(
                "normal_font",
                font
            );
        }

        label.BbcodeEnabled = true;

        label.AddThemeColorOverride(
            "default_color",
            Colors.White
        );

        label.AddThemeColorOverride(
            "font_outline_color",
            Colors.Black
        );

        label.AddThemeConstantOverride(
            "outline_size",
            8
        );

        label.AddThemeFontSizeOverride(
            "normal_font_size",
            32
        );

        label.MouseFilter =
            MouseFilterEnum.Ignore;
    }

    private void RefreshDisplay()
    {
        //
        // TEMP TEST VALUE
        //

        const int stockCount = 4;

        if (_orbLabel != null)
        {
            _orbLabel.Text =
                $"[center]{stockCount}[/center]";
        }
    }

    public override void _ExitTree()
    {
        _exiting = true;

        _orb = null;

        _card1 = null;
        _card2 = null;
        _card3 = null;
        _card4 = null;

        _orbLabel = null;

        _cardDisplay = null;
        _player = null;

        if (Instance == this)
            Instance = null;
    }
}

[HarmonyPatch(
    typeof(NEnergyCounter),
    nameof(NEnergyCounter._Ready)
)]
public static class CardDisplayOverlayPatch
{
    public static void Postfix(
        NEnergyCounter __instance)
    {
        if (__instance == null)
            return;

        if (!GodotObject.IsInstanceValid(__instance))
            return;

        if (__instance.IsQueuedForDeletion())
            return;

        if (__instance.GetNodeOrNull<CardDisplayOverlay>(
                "CardDisplayOverlay") != null)
            return;

        var overlay =
            new CardDisplayOverlay
            {
                Name = "CardDisplayOverlay"
            };

        __instance.AddChild(overlay);
    }
}