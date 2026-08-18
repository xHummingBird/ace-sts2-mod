// /**
// Red Card
//
// 1 Card: Burst - Hurl a magic projectile at the opponent.
//
// 2 Cards: Super Burst - Hurl a fast and powerful orb at the opponent.
//
// 3 Cards: Mega Burst - Cause a massive explosion to occur at the foe's position.
//
// Black Card
//
// 1 Card: Short Stop - Casts a dark orb that slowly pursues the opponent and
// briefly prevents them from moving on hit.
//
// 2 Cards: Long Stop - Cast a dark orb that slowly pursues the opponent and
// temporarily prevents them from moving on hit.
//
// 3 Cards: Mega Stop - Unleash an arcane explosion at the target's location and
// prevent them from moving for an extended period of time on hit.
//
// Blue Card
//
// 1 Card: Charge - Restore a small amount of nearby allies' HP and bravery.
//
// 2 Cards: Super Charge - Restore a moderate amount of nearby allies' HP and
// bravery.
//
// 3 Cards: Mega Charge - Restore a significant amount of nearby allies' HP and
// bravery.
//
// Yellow Card
//
// 1 Card: Force - Slightly raise the attack power, defense, and movement speed of
// all nearby allies for a short period.
//
// 2 Cards: Super Force - Moderately raise the attack power, defense, and movement
// speed of all nearby allies for a short period.
//
// 3 Cards: Mega Force - Significantly raise the attack power, defense, and
// movement speed of nearby allies for a short period.
// */
//
// using BaseLib.Extensions;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Powers;
// using MegaCrit.Sts2.Core.ValueProps;
//
// namespace Ace.AceCode.Mechanics;
//
// public readonly record struct FlipResultOld(AceColor? Color, int Consumed,
//                                          int Level) {
//   public bool Happened => Consumed > 0;
// }
//
// // Flip consumes stock cards and applies the standard effect for the color, the
// // strength depends on how many cards were taken. A card that wants its own
// // effect should use Consume instead.
// public static class FlipOld {
//   public const int MaxLevel = 3;
//
//   public static Task<FlipResult>
//   Majority(PlayerChoiceContext choiceContext, CardModel card, CardPlay play,
//            int bonus = 0) => Resolve(choiceContext, card, play,
//                                      Stock.Majority(card.Owner),
//                                      Consume.Majority(card.Owner), bonus);
//
//   public static Task<FlipResult>
//   Color(PlayerChoiceContext choiceContext, CardModel card, CardPlay play,
//         AceColor color,
//         int bonus = 0) => Resolve(choiceContext, card, play, color,
//                                   Consume.OfColor(card.Owner, color), bonus);
//
//   // The whole stock goes, but the effect still comes from a single color.
//   public static Task<FlipResult> All(PlayerChoiceContext choiceContext,
//                                      CardModel card, CardPlay play,
//                                      int bonus = 0) {
//     var color = Stock.Majority(card.Owner);
//     return Resolve(choiceContext, card, play, color, Consume.All(card.Owner),
//                    bonus);
//   }
//
//   private static async Task<FlipResult>
//   Resolve(PlayerChoiceContext choiceContext, CardModel card, CardPlay play,
//           AceColor? color, IReadOnlyList<AceColor> consumed, int bonus) {
//     if (color is not {} flipped || consumed.Count == 0)
//       return default;
//
//     var level = Math.Min(consumed.Count, MaxLevel);
//
//     switch (flipped) {
//     case AceColor.Red:
//       await Burst(choiceContext, card, play, level, bonus);
//       break;
//     case AceColor.White:
//       await Stop(choiceContext, card, play, level, bonus);
//       break;
//     case AceColor.Blue:
//       await Charge(card, level, bonus);
//       break;
//     case AceColor.Yellow:
//       await Force(choiceContext, card, level, bonus);
//       break;
//     }
//
//     return new FlipResult(flipped, consumed.Count, level);
//   }
//
//   private static async Task Burst(PlayerChoiceContext choiceContext,
//                                   CardModel card, CardPlay play, int level,
//                                   int bonus) {
//     if (play.Target is null)
//       return;
//
//     var damage = level switch {
//       1 => 8m,
//       2 => 14m,
//       _ => 22m
//     } + bonus;
//
//     await DamageCmd.Attack(damage)
//         .FromCard(card, play)
//         .Targeting(play.Target)
//         .WithValueProp(ValueProp.Unpowered)
//         .Execute(choiceContext);
//   }
//
//   // The Type-0 black card, white is what the mod calls it.
//   private static async Task Stop(PlayerChoiceContext choiceContext,
//                                  CardModel card, CardPlay play, int level,
//                                  int bonus) {
//     if (play.Target is null)
//       return;
//
//     if (level >= MaxLevel) {
//       await CreatureCmd.Stun(play.Target);
//       return;
//     }
//
//     var weak = (level == 1 ? 2m : 3m) + bonus;
//
//     await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, weak,
//                                     card.Owner.Creature, card);
//   }
//
//   private static async Task Charge(CardModel card, int level, int bonus) {
//     if (card.Owner.Creature is not {} ace)
//       return;
//
//     var heal = level switch {
//       1 => 4m,
//       2 => 8m,
//       _ => 14m
//     } + bonus;
//
//     await CreatureCmd.Heal(ace, heal);
//   }
//
//   private static async Task Force(PlayerChoiceContext choiceContext,
//                                   CardModel card, int level, int bonus) {
//     if (card.Owner.Creature is not {} ace)
//       return;
//
//     var amount = level + bonus;
//
//     await PowerCmd.Apply<StrengthPower>(choiceContext, ace, amount, ace, card);
//     await PowerCmd.Apply<DexterityPower>(choiceContext, ace, amount, ace, card);
//   }
// }
