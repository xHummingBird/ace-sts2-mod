using Ace.AceCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Powers;

public class CounterPlayPower : AcePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private sealed class Pending
    {
        public int Count;
    }

    protected override object InitInternalData() => new Pending();

    public override Task AfterBlockGained(
        Creature creature,
        decimal amount,
        ValueProp props,
        CardModel? cardSource)
    {
        if (creature == base.Owner && cardSource is AceBlueCard)
            GetInternalData<Pending>().Count++;

        return Task.CompletedTask;
    }

    // AfterBlockGained has no PlayerChoiceContext, so the damage is deferred to the end of the card
    // play that produced the block.
    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        var pending = GetInternalData<Pending>();

        if (pending.Count == 0)
            return;

        var triggers = pending.Count;
        pending.Count = 0;

        for (var i = 0; i < triggers; i++)
        {
            var targets = base.CombatState.HittableEnemies.ToList();

            if (targets.Count == 0)
                return;

            await CreatureCmd.Damage(
                choiceContext,
                targets,
                base.Amount,
                ValueProp.Move,
                base.Owner);
        }
    }
}
