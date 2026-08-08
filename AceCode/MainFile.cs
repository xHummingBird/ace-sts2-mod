using System.Reflection;
using Ace.AceCode.Mechanics;
using BaseLib.Patches.Saves;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Ace.AceCode
{
    [ModInitializer(nameof(Initialize))]
    public partial class MainFile : Node
    {
        public const string ModId = "Ace"; //Used for resource filepath
        public const string ResPath = $"res://{ModId}";

        public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
            new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

        public static void Initialize()
        {
            // Has to happen before BaseLib collects the saved fields, otherwise
            // the stock is dropped from the save and the rejoin payload.
            ExtendedSaveTypes.RegisterListSaveType<AceColor>();
            Stock.Register();

            Harmony harmony = new(ModId);
            var assembly = Assembly.GetExecutingAssembly();
            Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
            harmony.PatchAll(assembly);
        }
    }
}
