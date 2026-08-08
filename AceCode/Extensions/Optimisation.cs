using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Ace.AceCode.Extensions;

public static class AceAssets
{
    private static PackedScene? _aceScene;
    private static PackedScene? _vfxScene;

    private const string AceScenePath = "res://Ace/scenes/ace.tscn";
    private const string VfxPath = "res://Ace/scenes/vfx.tscn";

    public static PackedScene? AceScene
    {
        get
        {
            _aceScene = LoadOrReload(_aceScene, AceScenePath, "Ace scene");
            return _aceScene;
        }
    }

    public static PackedScene? IceScene
    {
        get
        {
            _vfxScene = LoadOrReload(_vfxScene, VfxPath, "Ice VFX");
            return _vfxScene;
        }
    }

    private static PackedScene? LoadOrReload(PackedScene? cachedScene, string path, string label)
    {
        if (cachedScene != null && GodotObject.IsInstanceValid(cachedScene))
            return cachedScene;

        GD.Print($"AceAssets: Loading {label} from {path}");

        var scene = GD.Load<PackedScene>(path);

        if (scene == null)
        {
            GD.PrintErr($"AceAssets: FAILED to load {label}: {path}");
            return null;
        }

        GD.Print($"AceAssets: Loaded {label}");
        return scene;
    }

    public static void EnsurePreloaded()
    {
        _ = AceScene;
        _ = IceScene;

        GD.Print("AceAssets: EnsurePreloaded finished");
    }
}

[HarmonyPatch(typeof(Hook), nameof(Hook.AfterActEntered))]
public static class AceAfterActEnteredPreloadPatch
{
    [HarmonyPrefix]
    public static void Prefix(IRunState runState)
    {
        var player = runState?.Players?.FirstOrDefault();

        if (player?.Character is not Character.Ace)
            return;

        GD.Print("AfterActEntered: Ace detected → preloading");

        AceAssets.EnsurePreloaded();
    }
}


[HarmonyPatch(typeof(Hook), nameof(Hook.AfterRoomEntered))]
public static class AceAfterRoomEnteredPreloadPatch
{
    [HarmonyPrefix]
    public static void Prefix(IRunState runState, AbstractRoom room)
    {
        var player = runState?.Players?.FirstOrDefault();

        if (player?.Character is not Character.Ace)
            return;

        GD.Print($"AfterRoomEntered: Ace detected → preloading. Room = {room.GetType().Name}");

        AceAssets.EnsurePreloaded();
    }
}
