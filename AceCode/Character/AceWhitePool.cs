namespace Ace.AceCode.Character;

public class AceWhitePool : AceColorPool
{
    public override string Title => $"{Ace.CharacterId}White";

    public override float H => 0f; //Hue; changes the color.
    public override float S => 0f; //Saturation
    public override float V => 1f; //Brightness
}
