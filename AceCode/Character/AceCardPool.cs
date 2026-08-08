using MegaCrit.Sts2.Core.Models;

namespace Ace.AceCode.Character;

public class AceCardPool : AceColorPool
{
    public override string Title => Ace.CharacterId; //This is not a display name.

    protected override CardModel[] GenerateAllCards() =>
    [
        .. ModelDb.CardPool<AceRedPool>().AllCards,
        .. ModelDb.CardPool<AceBluePool>().AllCards,
        .. ModelDb.CardPool<AceYellowPool>().AllCards,
        .. ModelDb.CardPool<AceWhitePool>().AllCards,
    ];
}
