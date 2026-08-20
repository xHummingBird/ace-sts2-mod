using Ace.AceCode.Cards;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace Ace.AceCode.Mechanics;

public partial class ColorCardDisplay : Control
{
	private static readonly Texture2D RedOrb =
		GD.Load<Texture2D>("res://Ace/images/card_ui/orb_red.png");

	private static readonly Texture2D BlueOrb =
		GD.Load<Texture2D>("res://Ace/images/card_ui/orb_blue.png");

	private static readonly Texture2D YellowOrb =
		GD.Load<Texture2D>("res://Ace/images/card_ui/orb_yellow.png");

	private static readonly Texture2D WhiteOrb =
		GD.Load<Texture2D>("res://Ace/images/card_ui/orb_white.png");

	private TextureRect _icon = null!;

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("Icon");
	}

	public void SetCard(CardModel model)
	{
		_icon.Texture = model switch
		{
			AceRedCard => RedOrb,
			AceBlueCard => BlueOrb,
			AceYellowCard => YellowOrb,
			AceWhiteCard => WhiteOrb,
			_ => null
		};
	}
}
