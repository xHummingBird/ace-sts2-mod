using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Ace.AceCode.Powers;

public class RaisingStakesPower : AcePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != base.Owner.Player)
            return;

        if (Stock.Majority(player) != AceColor.Red)
            return;

        await PowerCmd.Apply<VigorPower>(
            choiceContext,
            base.Owner,
            base.Amount,
            base.Owner,
            null);
    }
}
