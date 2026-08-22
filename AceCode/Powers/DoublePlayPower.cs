using Ace.AceCode.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Ace.AceCode.Powers;

public class DoublePlayPower : AcePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private sealed class TurnState
    {
        public int Used;
    }

    protected override object InitInternalData() => new TurnState();

    private bool HasCharge => GetInternalData<TurnState>().Used < base.Amount;

    private bool Applies(CardModel card) =>
        card is AceRedCard && card.Owner == base.Owner.Player;

    public override int ModifyCardPlayCount(
        CardModel card,
        Creature? target,
        int playCount)
    {
        return Applies(card) && HasCharge ? playCount + 1 : playCount;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        if (Applies(card) && HasCharge)
            GetInternalData<TurnState>().Used++;

        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player == base.Owner.Player)
            GetInternalData<TurnState>().Used = 0;

        return Task.CompletedTask;
    }
}
