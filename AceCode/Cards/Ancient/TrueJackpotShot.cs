using Ace.AceCode.Extensions;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Ancient;

public class TrueJackpotShot() : AceFlipCard(0, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(15m),
        new ExtraDamageVar(2m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel _, Creature? _) => CombatManager.Instance.History.CardPlaysFinished.Count())
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

        var targets = base.CombatState.HittableEnemies;
        
        void PlayFakeHitAll(
            IReadOnlyList<Creature> targets,
            Character.Ace ace)
        {
            foreach (var target in targets)
            {
                if (!target.IsAlive)
                    continue;
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target));
                AceExtensions.CombatHelpers.AceFakeHit(ownerCreature, target, null, null, null);
            }
        }
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            SfxCmd.Play("res://Ace/sounds/ultimate (2).wav");
            float duration = ace.PlayAnimation(ownerCreature, "true_jackpot_shot").total;
            SfxCmd.Play("res://Ace/sfx/card_shuffle_2.wav");
            await Task.Delay((int)(0.5f * 1000f));
            SfxCmd.Play("res://Ace/sfx/jackpot_charge.wav");
            await Task.Delay((int)(1.433f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_fire.wav");
            await Task.Delay((int)(0.267f * 1000f));
            
            PlayFakeHitAll(targets, ace);
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            
            await Task.Delay((int)(0.2f * 1000f));
            
            PlayFakeHitAll(targets, ace);
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            
            await Task.Delay((int)(0.2f * 1000f));
            
            PlayFakeHitAll(targets, ace);
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            
            await Task.Delay((int)(0.2f * 1000f));
            
            PlayFakeHitAll(targets, ace);
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            
            await Task.Delay((int)(0.2f * 1000f));
            
            PlayFakeHitAll(targets, ace);
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            
            await Task.Delay((int)(0.2f * 1000f));
            foreach (var target in targets)
            {
                if (!target.IsAlive)
                    continue;
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target));
            }
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "event:/sfx/characters/attack_fire")
                .Execute(choiceContext);
           
            await Task.Delay((int)(800f));
            
        }
        else
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "event:/sfx/characters/attack_fire")
                .Execute(choiceContext);
        CenterCardCinematic.End(RunManager.Instance.NetService.NetId);
    }

    protected override void OnUpgrade()
    {
    }
}