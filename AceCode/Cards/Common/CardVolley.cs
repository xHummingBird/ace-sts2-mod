using Ace.AceCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Common;

public class CardVolley() : AceRedCard(2, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move), new RepeatVar(3)];

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
            await CommonActions.CardAttack(this, play, hitCount: DynamicVars.Repeat.IntValue).WithHitFx(null, "res://Ace/sfx/card_hit.wav").Execute(choiceContext);
            await Task.Delay((int)(0.2f * 1000f));
        };
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}
