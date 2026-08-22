using BaseLib.Abstracts;
using BaseLib.Extensions;
using Ace.AceCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Localization;

namespace Ace.AceCode.Powers
{
    public abstract class AcePower : CustomPowerModel
    {
        //Loads from Ace/images/powers/your_power.png
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    }
    
}
