using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Flip;

public class MegaBurst() : AceRedCard(0, CardType.Attack,
    CardRarity.Token, TargetType.AllEnemies), IFlipCard
{
    public override bool CanBeGeneratedInCombat => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(16m, ValueProp.Move),
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            
            AudioHelper.PlayRandomAttackCritical();
            float duration = ace.PlayAnimation(ownerCreature, "cast").total;
            var enemies = CombatState.HittableEnemies;
            foreach (var e in enemies)
                ace.PlayVfxOnTarget(
                    e,
                    "res://Ace/scenes/vfx.tscn",
                    "burst"
                );
            SfxCmd.Play("res://Ace/sfx/burst_2.wav");
            await Task.Delay((int)(0.40f * 1000f));
        }
        
        await CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null, "res://Ace/sfx/burst_damage.wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.40f * 1000f));
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}