using Ace.AceCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare
{
public class SpiralGambit() : AceBlueCard(3, CardType.Attack,
        CardRarity.Rare, TargetType.AnyEnemy)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(18m, ValueProp.Move),
            new EnergyVar(1)
        ];
        
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
        {
            var ownerCreature = Owner?.Creature;
            
            CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
            if (ownerCreature != null && Owner?.Character is Character.Ace ace)
            {
                AudioHelper.PlayRandomLimitBreak();
                float duration = ace.PlayAnimation(ownerCreature, "cast").total;
                ace.PlayVfxOnTarget(
                    play.Target,
                    "res://Ace/scenes/vfx.tscn",
                    "spiral_gambit"
                );
                SfxCmd.Play("res://Ace/sfx/card_flip.wav");
                await Task.Delay((int)(0.333f * 1000f));
                SfxCmd.Play("res://Ace/sfx/ragnarok_shoot.wav");
                await Task.Delay((int)(0.2f * 1000f));
                
            }
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(play.Target));
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "event:/sfx/characters/attack_fire")
                .Execute(choiceContext);
            await Task.Delay((int)(0.5f * 1000f));
            CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
        public override Task AfterCardEnteredCombat(CardModel card)
        {
            if (card != this)
            {
                return Task.CompletedTask;
            }
            if (base.IsClone)
            {
                return Task.CompletedTask;
            }
            int amount = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Player == base.Owner && e.HappenedThisTurn(base.CombatState));
            ReduceCostBy(amount);
            return Task.CompletedTask;
        }

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner != base.Owner)
            {
                return Task.CompletedTask;
            }
            if (cardPlay.Card.Type != CardType.Skill)
            {
                return Task.CompletedTask;
            }
            ReduceCostBy(1);
            return Task.CompletedTask;
        }

        private void ReduceCostBy(int amount)
        {
            base.EnergyCost.AddThisTurn(-amount);
        }
    }
}
