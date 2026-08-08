using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Ace.AceCode.Mechanics {
public class AceStockModel() : CustomSingletonModel
(HookType.Combat) {
  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext,
                                       CardPlay cardPlay) {
    // The singleton listens in every combat, so other characters have to be
    // filtered out.
    if (cardPlay.Player.Character is not Character.Ace)
      return Task.CompletedTask;

    if (cardPlay.Card is not IStockingCard card)
      return Task.CompletedTask;

    Stock.Push(cardPlay.Player, card.StockColor);
    return Task.CompletedTask;
  }
}
}
