using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Powers;

public class BreaksightPower : AcePower
{
    private const string _damageDecrease = "DamageDecrease";
    
    private const string _damageIncrease = "DamageIncrease";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("DamageDecrease", 0.25m),
        new DynamicVar("DamageIncrease", 1.75m)
    ];
    
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }

        //
        // Weak portion
        // Owner deals 75% less damage
        //
        if (dealer == Owner)
        {
            return 1m; // 0.25
        }

        //
        // Vulnerable portion
        // Owner takes 75% more damage
        //
        if (target == Owner)
        {
            return DynamicVars["DamageIncrease"].BaseValue; // 1.75
        }

        return 1m;
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.TickDownDuration(this);
        }
    }
}