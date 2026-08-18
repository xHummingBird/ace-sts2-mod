using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Ace.AceCode.Cards.Ancient;

public class FalseCut(): AceCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];
        
protected override IEnumerable<IHoverTip> ExtraHoverTips =>
[
    AceStaticHoverTip.Stock,
];

protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
{
    var cards = (await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, base.Owner));
    foreach (var card in cards)
    {
        var color = Stock.GetColor(card);

        if (color is not null)
            Stock.Push(base.Owner, color.Value);
    }
}

protected override void OnUpgrade()
{
    AddKeyword(CardKeyword.Retain);
}
}
