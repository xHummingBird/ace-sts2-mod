using Ace.AceCode.Character;
using Ace.AceCode.Extensions;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Basic
{
    public class StrikeYellow() : AceYellowCard(1, CardType.Attack,
        CardRarity.Basic, TargetType.AnyEnemy)
    {
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(3m, ValueProp.Move),
            new RepeatVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
        {
            var ownerCreature = Owner?.Creature;

            if (ownerCreature != null && Owner?.Character is Character.Ace ace)
            {
                decimal damage = DynamicVars.Damage.PreviewValue;
                AudioHelper.PlayRandomAttack();
                float duration = ace.PlayAnimation(ownerCreature, "double_strike").total;
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
                CommonActions.CardAttack(this, play.Target)
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
                await CommonActions.CardAttack(this, play.Target)
                    .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                    .Execute(choiceContext);
                await Task.Delay((int)(0.2f * 1000f));
            }
            else
                await CommonActions.CardAttack(this, play.Target, hitCount: DynamicVars.Repeat.IntValue)
                    .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                    .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}
