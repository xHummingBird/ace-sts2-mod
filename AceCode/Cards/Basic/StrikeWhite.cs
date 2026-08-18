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

namespace Ace.AceCode.Cards.Basic
{
    public class StrikeWhite() : AceWhiteCard(1, CardType.Attack,
        CardRarity.Basic, TargetType.AnyEnemy), IStockingCard
    {
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(4m, ValueProp.Move),
            new PowerVar<WeakPower>(1m)
        ];
    
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<WeakPower>(),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
        {
            var ownerCreature = Owner?.Creature;

            if (ownerCreature != null && Owner?.Character is Character.Ace ace)
            {
                decimal damage = DynamicVars.Damage.PreviewValue;
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
                await CommonActions.CardAttack(this, play.Target)
                    .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                    .Execute(choiceContext);
                await Task.Delay((int)(0.2f * 1000f));
                await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
            };
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
            DynamicVars.Weak.UpgradeValueBy(1m);
        }
    }
}
