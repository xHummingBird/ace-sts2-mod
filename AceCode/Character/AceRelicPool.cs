using BaseLib.Abstracts;
using Ace.AceCode.Extensions;
using Godot;

namespace Ace.AceCode.Character;

public class AceRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Ace.Color;

    public override string BigEnergyIconPath => "charui/energy_card.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}