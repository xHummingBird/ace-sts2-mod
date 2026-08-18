using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Relics;

public class AkedemeiaDeck() : AceRelic
{
    public const int Damage = 4;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    private int _pending;

    // The consume path is synchronous and has no PlayerChoiceContext, so the damage waits for the
    // end of the card play that consumed the Stock.
    public void QueueTrigger() => _pending++;

    public override Task BeforeCombatStart()
    {
        _pending = 0;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (_pending == 0)
            return;

        var triggers = _pending;
        _pending = 0;

        if (base.Owner?.Creature is not { } dealer)
            return;

        for (var i = 0; i < triggers; i++)
        {
            var targets = dealer.CombatState.HittableEnemies.ToList();

            if (targets.Count == 0)
                return;

            await CreatureCmd.Damage(
                choiceContext,
                targets,
                Damage,
                ValueProp.Move,
                dealer);
        }
    }
}
