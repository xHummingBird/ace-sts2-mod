# Ace

Slay the Spire 2 mod adding the Ace character (FF TYPE:0). Requires BaseLib v3.4.0+ and game
version 0.109.0+.

## Building

Open `Ace.sln` in Rider and build, or from the CLI:

```
dotnet build Ace.sln                       # deploys the .dll to <game>/mods/Ace/
dotnet publish Ace.csproj -c ExportRelease # also exports the Godot .pck
```

`Sts2PathDiscovery.props` finds the game automatically from the Steam registry keys / default
library paths. Override it with an untracked `local.props` (imported by `Directory.Build.props`) or
`-p:Sts2Path=...`.

`ExportRelease` needs MegaDot 4.5.1 at the path in `Directory.Build.props` — the game will refuse a
`.pck` built by a newer Godot.

## Developing from WSL (Neovim + roslyn_ls)

C# IntelliSense works from WSL against the Windows game install. One-time machine setup:

1. Install the .NET SDKs and `libicu`:
   ```
   curl -fsSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 9.0
   curl -fsSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
   sudo apt install -y libicu78
   ```
   9.x builds the project. 10.x is separately required because every published
   `roslyn-language-server` nuget ships its payload under `tools/net10.0/`; with a 9-only SDK the
   install fails with a misleading *"DotnetToolSettings.xml was not found in the package"*.
2. Install the server: `:MasonInstall roslyn-language-server` (nvim-lspconfig ships a complete
   `roslyn_ls` config — no `roslyn.nvim` plugin needed), then enable it in your LSP config.
3. Run `sts2-mod-wsl-setup`.

Step 3 is the one that isn't obvious. Under Linux, `Sts2PathDiscovery.props` looks for
`$SteamLibraryPath/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64`, which does not exist when the
game is installed on the Windows side. `CheckDependencyPaths` then hard-errors, and the language
server attaches to an empty workspace — which looks like it is working. The script builds
`~/sts2-shim/` with a symlink of that Linux name pointing at the real `data_sts2_windows_x86_64`
(managed MSIL, so cross-platform referencing is fine) and exports `SteamLibraryPath` at it. That is
the only property in the Linux block assigned conditionally, so it's the only one an environment
variable can override — which is why this needs no changes to the project files.

Any STS2 mod project works after that, with no per-project setup. **This** repo additionally carries
a `local.props` sending WSL output to `obj-wsl/`, so Rider and WSL don't invalidate each other's
`project.assets.json`; that part is optional and not needed in a fresh clone.

Run `sts2-mod-wsl-setup --check` to diagnose if IntelliSense goes quiet. `dotnet restore` at the repo
root reproduces the underlying error directly.
