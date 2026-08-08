namespace Ace.AceCode.Character;

public class AceYellowPool : AceColorPool
{
    public override string Title => $"{Ace.CharacterId}Yellow";

    public override float H => 0.14f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1.1f; //Brightness
}
