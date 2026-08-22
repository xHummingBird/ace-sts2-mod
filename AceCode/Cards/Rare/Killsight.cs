using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using Ace.AceCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class Killsight() : AceWhiteCard(1, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy), IStockingCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10, ValueProp.Move),
        new DynamicVar("hpPercent", 10)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<BreaksightPower>(),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;
        decimal threshold = DynamicVars["hpPercent"].BaseValue;
        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            AudioHelper.PlayRandomAttackCritical();
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
        if (play.Target.CurrentHp * 100 <= play.Target.MaxHp * threshold && play.Target.CurrentHp > 0)
        {
            await DoomPower.DoomKill(new List<Creature> {play.Target});
            return;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars["hpPercent"].UpgradeValueBy(2);
    }
}