using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class LastCard() : AceYellowCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override bool ShouldGlowGoldInternal => PileType.Draw.GetPile(base.Owner).Cards.Count == 0;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move), new PowerVar<WeakPower>(1m)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (PileType.Draw.GetPile(base.Owner).Cards.Count == 0) 
            await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Yellow, 1);
        else await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Yellow);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
