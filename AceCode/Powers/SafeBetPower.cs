using Ace.AceCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Powers;

public class SafeBetPower : AcePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private sealed class TurnState
    {
        public int Used;
    }

    protected override object InitInternalData() => new TurnState();

    private bool HasCharge => GetInternalData<TurnState>().Used < base.Amount;

    public override decimal ModifyBlockMultiplicative(
        Creature target,
        decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (target != base.Owner || cardSource is not AceBlueCard)
            return 1m;

        return HasCharge ? 2m : 1m;
    }

    // The multiplicative hook also runs to preview block on cards still in hand, where cardPlay is
    // null. Only a real play may spend a charge.
    public override Task AfterModifyingBlockAmount(
        decimal modifiedAmount,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardPlay is not null
            && cardSource is AceBlueCard
            && cardSource.Owner == base.Owner.Player
            && HasCharge)
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
