using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class Memento() : AceBlueCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IFlipCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => (card.Owner.Creature.Block + (Stock.Count(card.Owner, AceColor.Blue) * 3)))
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var ownerCreature = Owner?.Creature;

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

        await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, play).Targeting(play.Target)
            .WithHitFx(null, "res://Ace/sfx/card_hit.wav")
            .Execute(choiceContext);
        await Task.Delay((int)(0.2f * 1000f));
        if (Stock.Count(Owner, AceColor.Blue) >= 1)
            SfxCmd.Play("res://Ace/sounds/open.wav");
        await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Blue);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
