using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare
{
public class BlizzagaBom() : AceBlueCard(2, CardType.Attack,
        CardRarity.Rare, TargetType.AllEnemies)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(12m, ValueProp.Move),
            new BlockVar(12m, ValueProp.Move),
            new EnergyVar(1)
        ];

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay play)
        {
            var ownerCreature = Owner?.Creature;
            var targets = CombatState.HittableEnemies;
            if (ownerCreature != null && Owner?.Character is Character.Ace ace)
            {
                
                AudioHelper.PlayRandomIce();
                float duration = ace.PlayAnimation(ownerCreature, "cast").total;
                foreach (var target in targets) {
                    ace.PlayVfxOnTarget(
                        target,
                        "res://Ace/scenes/vfx.tscn",
                        "ice_1"
                    );
                }
                await Task.Delay((int)(0.20f * 1000f));
            }
        
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "res://Ace/sfx/ice.wav")
                .BeforeDamage(async delegate
                {
                    foreach (var target in targets)
                    {
                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(
                            NGroundFireVfx.Create(target, VfxColor.Blue));
                    }
                })
                .Execute(choiceContext);
            await CommonActions.CardBlock(this, play);
            var majority = Stock.Majority(base.Owner);

            if (majority == AceColor.Blue)
            {
                await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, base.Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1m);
        }
    }
}
