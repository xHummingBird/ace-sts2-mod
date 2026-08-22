using Ace.AceCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Ancient;

public class UltimateBurst() : AceCard(0, CardType.Attack,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(40m, ValueProp.Move),
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

        var targets = base.CombatState.HittableEnemies;
        
        CenterCardCinematic.Start(RunManager.Instance.NetService.NetId);
        if (ownerCreature != null && Owner?.Character is Character.Ace ace)
        {
            AudioHelper.PlayRandomLimitBreak();
            float duration = ace.PlayAnimation(ownerCreature, "ultimate_burst").total;
            SfxCmd.Play("res://Ace/sfx/card_charge.wav");
            await Task.Delay((int)(1f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_explosion_long.wav");
            await Task.Delay((int)(1f * 1000f));
            SfxCmd.Play("res://Ace/sfx/card_explosion_long.wav");
            
            await Task.Delay((int)(0.5f * 1000f));
            foreach (var target in targets)
            {
                if (!target.IsAlive)
                    continue;
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target));
            }
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "event:/sfx/characters/attack_fire")
                .Execute(choiceContext);
            await Task.Delay((int)(1f * 1000f));
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