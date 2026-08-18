using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

//hit 3 random target, 5 damage. Increase hit count for each yellow card stocked
public class ChainLightning() : AceYellowCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy), IConsumeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(4)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        int repeat = DynamicVars.Repeat.IntValue + Stock.Count(base.Owner, AceColor.Yellow);

        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            AudioHelper.PlayRandomThunder();
            float duration = ace.PlayAnimation(ownerCreature, "cast").total;
            if (duration > 0f)
                await Task.Delay((int)(0.2f * 1000f));
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .TargetingRandomOpponents(base.CombatState)
            .WithHitCount(repeat)
            .WithHitFx("vfx/vfx_attack_lightning", "event:/sfx/characters/defect/defect_lightning_passive")
            .Execute(choiceContext);
        Consume.OfColor(Owner, AceColor.Yellow);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}
