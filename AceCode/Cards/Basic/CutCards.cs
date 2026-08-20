using Ace.AceCode.Character;
using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Basic
{ 
public class CutCards() : AceCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
        
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            AceStaticHoverTip.Stock,
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
        {
            SfxCmd.Play("res://Ace/sounds/draw.wav");
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
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}
