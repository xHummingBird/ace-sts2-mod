using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare
{
public class Bioga() : AceWhiteCard(2, CardType.Attack,
        CardRarity.Rare, TargetType.AnyEnemy), IStockingCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(12m, ValueProp.Move),
            new PowerVar<VulnerablePower>(2m),
            new PowerVar<WeakPower>(2m)
        ];
    
        protected override async Task OnPlay(PlayerChoiceContext choiceContext,
            CardPlay play)
        {
            var ownerCreature = Owner?.Creature;

            if (ownerCreature != null && Owner?.Character is Character.Ace ace)
            {
                AudioHelper.PlayRandomFire();
                float duration = ace.PlayAnimation(ownerCreature, "cast").total;
                if (duration > 0f)
                    await Task.Delay((int)(0.2f * 1000f));
            }
            await CommonActions.CardAttack(this, play.Target)
                .BeforeDamage(async delegate
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(play.Target, VfxColor.Green));
                    SfxCmd.Play("event:/sfx/characters/attack_fire");
                })
                .Execute(choiceContext);
            int powerAmount = Stock.Count(Owner, AceColor.White) + 2;
            
            await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, powerAmount, base.Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, powerAmount, base.Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
