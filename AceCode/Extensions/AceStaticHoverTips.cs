using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Ace.AceCode.Extensions;

public static class AceStaticHoverTip
{
    public static readonly IHoverTip Stock = new HoverTip(
        new LocString("static_hover_tips", "ACE_STOCK.title"),
        new LocString("static_hover_tips", "ACE_STOCK.description")
    );

    public static readonly IHoverTip Flip = new HoverTip(
        new LocString("static_hover_tips", "ACE_FLIP.title"),
        new LocString("static_hover_tips", "ACE_FLIP.description")
    );

    public static readonly IHoverTip Unstockable = new HoverTip(
        new LocString("static_hover_tips", "ACE_UNSTOCKABLE.title"),
        new LocString("static_hover_tips", "ACE_UNSTOCKABLE.description")
    );
    
    public static readonly IHoverTip Majority = new HoverTip(
        new LocString("static_hover_tips", "ACE_MAJORITY.title"),
        new LocString("static_hover_tips", "ACE_MAJORITY.description")
    );
}