using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

public class LoadedDeck() : AceCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CalculationBaseVar(3m),
        new ExtraDamageVar(3m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => Stock.Count(card.Owner))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        {
            var ownerCreature = Owner?.Creature;
            var targets = base.CombatState.HittableEnemies;
            if (ownerCreature != null && Owner?.Character is Character.Ace ace)
            {
                AudioHelper.PlayRandomAttackMedium();
                float duration = ace.PlayAnimation(ownerCreature, "card_fan").total;
                await Task.Delay((int)(0.267f * 1000f));
                SfxCmd.Play("res://Ace/sfx/card_throw.wav");
                await Task.Delay((int)(0.1f * 1000f));
            
                foreach (var target in targets)
                    ace.PlayVfxOnTarget(
                        target,
                        "res://Ace/scenes/vfx.tscn",
                        "hit"
                    );
            };
            await CommonActions.CardAttack(this, play.Target)
                .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
                .Execute(choiceContext);
            await Task.Delay((int)(0.2f * 1000f));
        }
        Consume.All(base.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(1m);
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}
