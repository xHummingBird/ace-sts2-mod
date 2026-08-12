# Ace (FF Type-0) — Slay the Spire 2 mod

## Comment style (strict)

Do not add comments to existing code. New comments only when the WHY is genuinely non-obvious, and
keep them minimal — the user removes/rewrites comments by hand afterward, in their own non-native,
non-colloquial English. Never write explanatory comments the user didn't ask for, never restate what
the code does.

## Build

```
~/.dotnet/dotnet build Ace.csproj                                          # WSL compile check only
"/mnt/c/Program Files/dotnet/dotnet.exe" build 'D:\Projects\RiderProjects\ace-sts2-mod\Ace.sln'   # deploys dll to mods folder
```

- The WSL build always fails the final copy step (can't reach the Windows-side mods folder) — that
  is expected; treat `error MSB3021/MSB3027` on `CopyToModsFolderOnBuild` as noise if compile
  succeeded above it.
- **Close Slay the Spire 2 before the Windows build** — a running game locks `mods/Ace/Ace.dll` and
  the copy fails.
- Publish (`dotnet.exe publish ... -c ExportRelease`, runs a `.pck` export) is only needed when
  something under `Ace/**` (scenes, images, localization-adjacent Godot resources) changed. Pure
  `.cs` changes only need `build`.
- Prefer solving things in `.cs` over editing `.tscn` — a scene change forces the slow Godot export;
  a code-only change doesn't.

## Reference sources (read these instead of guessing signatures)

- Decompiled game source (READ-ONLY): `/mnt/d/Projects/Godot Projects/Slay the Spire 2/src/` — full
  C# for `OrbCmd`, `PowerCmd`, `CreatureCmd`, `DamageCmd`, hooks (`AbstractModel.cs`, `Hook.cs`),
  multiplayer/rejoin (`src/Core/Multiplayer/`). Also has every vanilla `.tscn` under `scenes/` —
  open in MegaDot (`C:\megadot\...`) via Project Manager → Import → that folder's `project.godot`.
- BaseLib source: `/mnt/d/Projects/RiderProjects/BaseLib-StS2/` — `CustomOrbModel`, `CustomPowerModel`,
  `CustomSingletonModel`, `SpireField`/`NotNullSpireField`/`SavedSpireField`, `CommonActions`,
  `ExtendedSaveTypes`.
- Card ID convention: `ClassName` → `ACE-SCREAMING_SNAKE_CASE`. Every card needs
  `ACE-X.title`/`ACE-X.description` in `Ace/localization/eng/cards.json` or the build's analyzer
  fails it (`Ace.csproj` feeds that dir in as `AdditionalFiles`).

## Architecture

- **Stock** (`AceCode/Mechanics/Stock.cs`) — per-player `List<AceColor>`, max 4, oldest evicted on
  overflow. Backed by `SavedSpireField<Player, List<AceColor>>` (rides in the multiplayer rejoin
  payload via `SerializablePlayer`) — NOT `PlayerCombatState` (has no serialization at all) and NOT
  vanilla orbs (dropped intentionally; costs desync-checksum coverage, see below). Because `Player`
  is run-scoped, `AceStockModel.BeforeCombatStart()` clears it explicitly every combat.
- **Consume** (`AceCode/Mechanics/Consume.cs`) — raw stock removal (`Majority`/`OfColor`/`All`/
  `Last`/`First`), synchronous, returns what it removed. No VFX.
- **Flip** (`AceCode/Mechanics/Flip.cs`) — the shared keyword payoff on top of `Consume`; level =
  `min(consumed, 3)`. Cards wanting bespoke effects call `Consume` directly instead.
- **`CardDisplayOverlay`** — read-only renderer next to the energy counter. Must never call
  `Stock.Push`/`Consume`/`Flip` itself.
- `AceColor.White` is the Type-0 "Black" card — intentionally not renamed.

## Multiplayer constraint

Combat is deterministic lockstep (every peer simulates every player). Mutate `Stock` only from
awaited hook/command paths (`AfterCardPlayed`, `OnPlay`, `BeforeCombatStart`) — never from `_Process`,
UI, or anything gated on `LocalContext.IsMe`. `ChecksumTracker` only hashes `NetFullCombatState`, so
`Stock` (on `Player`, not in that struct) has no desync detection — divergence would be silent.
Mid-combat rejoin restore does not exist client-side in this game build
(`NotImplementedException` in the decompiled source) regardless of what's in the payload.
