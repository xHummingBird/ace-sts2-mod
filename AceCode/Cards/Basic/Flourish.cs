using Ace.AceCode.Character;
using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Basic;

public class Flourish() : AceCard(2, CardType.Attack,
    CardRarity.Basic, TargetType.AnyEnemy), IFlipCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4m, ValueProp.Move),
        new RepeatVar(3)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            decimal damage = DynamicVars.Damage.PreviewValue;
            AudioHelper.PlayRandomAttack();
            float duration = ace.PlayAnimation(ownerCreature, "flourish").total;
            await Task.Delay((int)(0.2f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_throw.wav");
            await Task.Delay((int)(0.1f * 1000f));
            ace.PlayVfxOnTarget(
                play.Target,
                "res://Ace/scenes/vfx.tscn",
                "card_hit_1"
            );
            DamageCmd.Attack(damage).FromCard(this, play).Targeting(play.Target)
                .WithValueProp(ValueProp.Unpowered)
                .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                .Execute(choiceContext);
            
            await Task.Delay((int)(0.2f * 1000f));
            AudioHelper.PlayRandomAttack();
            await Task.Delay((int)(0.267f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_throw.wav");
            await Task.Delay((int)(0.1f * 1000f));
            ace.PlayVfxOnTarget(
                play.Target,
                "res://Ace/scenes/vfx.tscn",
                "card_hit_2"
            );
            DamageCmd.Attack(damage).FromCard(this, play).Targeting(play.Target)
                .WithValueProp(ValueProp.Unpowered)
                .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                .Execute(choiceContext);
            AudioHelper.PlayRandomAttackHard();
            await Task.Delay((int)(0.367f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_throw.wav");
            await Task.Delay((int)(0.1f * 1000f));
            ace.PlayVfxOnTarget(
                play.Target,
                "res://Ace/scenes/vfx.tscn",
                "card_hit_3"
            );
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                .Execute(choiceContext);
           
            await Task.Delay((int)(0.25f * 1000f));
            
        }
        else
            await CommonActions.CardAttack(this, play.Target, hitCount: DynamicVars.Repeat.IntValue)
                .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                .Execute(choiceContext);
        await Ace.AceCode.Mechanics.Flip.Oldest(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}