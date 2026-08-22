using Ace.AceCode.Cards;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Ace.AceCode.Mechanics;

public static class ColorCardDisplayUi
{
    private const string NodeName = "ColorCard_UI";
    private const string ScenePath = "res://Ace/scenes/ColorCardDisplay.tscn";

    public static void Ensure(NCard card)
    {
        var model = card.Model;
        var body = card.Body;

        if (model == null || body == null)
            return;

        var node = body.GetNodeOrNull<ColorCardDisplay>(NodeName);

        if (node == null)
        {
            var scene = GD.Load<PackedScene>(ScenePath);
            if (scene == null)
                return;

            node = scene.Instantiate<ColorCardDisplay>();
            node.Name = NodeName;
            node.MouseFilter = Control.MouseFilterEnum.Ignore;

            body.AddChild(node);
        }

        bool isColorCard =
            model is AceRedCard ||
            model is AceBlueCard ||
            model is AceYellowCard ||
            model is AceWhiteCard;

        node.Visible = isColorCard;

        if (isColorCard)
        {
            node.SetCard(model);
        }

        node.Position = new Vector2(75f, -205f);
    }
}