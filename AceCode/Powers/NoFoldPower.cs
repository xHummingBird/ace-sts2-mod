using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Ace.AceCode.Powers;

public class NoFoldPower : AcePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldFlush(Player player) => player != base.Owner.Player;
}
