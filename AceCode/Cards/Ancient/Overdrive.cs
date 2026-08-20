using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Ace.AceCode.Cards.Ancient;

public class Overdrive() : AceCard(0, CardType.Skill,
    CardRarity.Ancient, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, 2, base.Owner);
        foreach (CardModel card in PileType.Hand.GetPile(base.Owner).Cards.ToList())
        {
            if (!card.EnergyCost.CostsX)
            {
                card.SetToFreeThisTurn();
            }
        }
    }
}