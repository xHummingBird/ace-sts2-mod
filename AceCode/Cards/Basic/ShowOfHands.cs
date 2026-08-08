using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Ace.AceCode.Cards.Basic;

// Colorless on purpose. A colored card would stock its own color right after
// the flip, because AfterCardPlayed runs after OnPlay.
public class ShowOfHands()
    : AceCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) {
  protected override async Task OnPlay(PlayerChoiceContext choiceContext,
                                       CardPlay play) {
    await Flip.Majority(choiceContext, this, play, IsUpgraded ? 2 : 0);
  }
}
