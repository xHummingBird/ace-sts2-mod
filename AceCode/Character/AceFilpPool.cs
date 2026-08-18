using Ace.AceCode.Extensions;
using BaseLib.Abstracts;
using Godot;

namespace Ace.AceCode.Character;

public class AceFlipPool : CustomCardPoolModel
{
    public override string Title => Ace.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/energy_card.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    public override float H => 1f; //Hue; changes the color.
    public override float S => 0f; //Saturation
    public override float V => 0.4f; //Brightness
    
    public override Color DeckEntryCardColor => new(Colors.Black);
    public override bool IsShared => true;
    public override bool IsColorless => false;
    
}

