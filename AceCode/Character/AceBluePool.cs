namespace Ace.AceCode.Character;

public class AceBluePool : AceColorPool
{
    public override string Title => $"{Ace.CharacterId}Blue";

    public override float H => 0.6f; //Hue; changes the color.
    public override float S => 1.15f; //Saturation
    public override float V => 1.05f; //Brightness
}
