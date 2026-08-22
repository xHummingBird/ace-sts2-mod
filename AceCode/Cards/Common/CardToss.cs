using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Common;

public class CardToss() : AceRedCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), IFlipCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Stock,
        AceStaticHoverTip.Unstockable,
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            AudioHelper.PlayRandomAttack();
            float duration = ace.PlayAnimation(ownerCreature, "attack").total;
            await Task.Delay((int)(0.2f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_throw.wav");
            await Task.Delay((int)(0.1f * 1000f));
            ace.PlayVfxOnTarget(
                play.Target,
                "res://Ace/scenes/vfx.tscn",
                "card_hit_1"
            );
        }
        await CommonActions.CardAttack(this, play.Target)
            .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.2f * 1000f));
        if (Stock.Count(Owner, AceColor.Red) >= 1)
        {
            SfxCmd.Play("res://Ace/sounds/open.wav");
            await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Red, 0, 1);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
