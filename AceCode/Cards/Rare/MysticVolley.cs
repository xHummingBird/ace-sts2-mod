using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class MysticVolley() : AceCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override bool ShouldGlowGoldInternal => Stock.IsRainbow(base.Owner);
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(16m, ValueProp.Move),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        var targets = base.CombatState.HittableEnemies;
        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            AudioHelper.PlayRandomAttackMedium();
            float duration = ace.PlayAnimation(ownerCreature, "card_fan").total;
            await Task.Delay((int)(0.267f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_throw.wav");
            await Task.Delay((int)(0.1f * 1000f));
            
            foreach (var target in targets)
                ace.PlayVfxOnTarget(
                    target,
                    "res://Ace/scenes/vfx.tscn",
                    "hit"
                );
        };
        await CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.2f * 1000f));
        if (Stock.IsRainbow(Owner))
        {
            await Ace.AceCode.Mechanics.Flip.Spectrum(
                choiceContext,
                this,
                play);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }
}