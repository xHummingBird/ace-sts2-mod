using Ace.AceCode.Extensions;
using Godot;

namespace Ace.AceCode.Character;

public class AceWhitePool : AceCardPool
{
    public override string Title => Ace.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/energy_card.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    
    public override float H => 0f; //Hue; changes the color.
    public override float S => 0f; //Saturation
    public override float V => 2.4f; //Brightness
    
    public override Color DeckEntryCardColor => new(Colors.White);
    public override bool IsShared => true;
    public override bool IsColorless => false;
}