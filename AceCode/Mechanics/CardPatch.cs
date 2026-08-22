using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Ace.AceCode.Mechanics;

[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateVisuals))]
public static class CardUiUpdate
{
    public static void Postfix(NCard __instance)
    {
        ColorCardDisplayUi.Ensure(__instance);
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard._Ready))]
public static class CardUiReady
{
    public static void Postfix(NCard __instance)
    {
        Callable.From(() =>
        {
            ColorCardDisplayUi.Ensure(__instance);
        }).CallDeferred();
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard._Ready))]
public static class CardUiModelChanged
{
    public static void Postfix(NCard __instance)
    {
        __instance.ModelChanged += _ =>
        {
            Callable.From(() =>
            {
                ColorCardDisplayUi.Ensure(__instance);

                // second pass
                Callable.From(() =>
                {
                    ColorCardDisplayUi.Ensure(__instance);
                }).CallDeferred();

            }).CallDeferred();
        };
    }
}