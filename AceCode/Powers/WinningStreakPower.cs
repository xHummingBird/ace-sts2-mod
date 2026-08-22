using Ace.AceCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Ace.AceCode.Powers;

public class WinningStreakPower : AcePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private sealed class TurnState
    {
        public bool Triggered;
    }

    protected override object InitInternalData() => new TurnState();

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        var state = GetInternalData<TurnState>();

        if (state.Triggered)
            return;

        if (cardPlay.Player != base.Owner.Player || cardPlay.Card is not AceRedCard)
            return;

        state.Triggered = true;

        await PowerCmd.Apply<StrengthPower>(
            choiceContext,
            base.Owner,
            base.Amount,
            base.Owner,
            null);
    }

    public override Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player == base.Owner.Player)
            GetInternalData<TurnState>().Triggered = false;

        return Task.CompletedTask;
    }
}
