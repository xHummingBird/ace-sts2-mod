namespace Ace.AceCode.Character;

public class AceRedPool : AceColorPool
{
    public override string Title => $"{Ace.CharacterId}Red";

    public override float H => 0f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness
}
