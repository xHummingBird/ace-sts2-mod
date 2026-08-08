using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Ace.AceCode.Extensions;

public static class AceExtensions
{
    public static class CombatHelpers
    {
        private const string DefaultHitVfx =
            "res://Ace/scenes/vfx.tscn";

        public static async Task AceFakeHit(
            Creature attacker,
            Creature target,
            string? hitSfx,
            string animName,
            string? hitVfx = null)
        {
            if (target == null)
                return;

            // attack sound
            SfxCmd.Play(hitSfx);
            var ace = attacker.Player.Character as Character.Ace;

            // impact effect
            ace.PlayVfxOnTarget(
                target,
                hitVfx ?? DefaultHitVfx,
                animName);

            // victim reaction
            await CreatureCmd.TriggerAnim(target, "Hit", 0f);

            if (target.Monster?.HasHurtSfx == true)
            {
                SfxCmd.Play(target.Monster.HurtSfx);
            }
        }
    }
}
