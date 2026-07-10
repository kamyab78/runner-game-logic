# Hyper Casual Runner Engine

Reusable Unity Package Manager (UPM) library for lane-based hyper-casual forward runners.

Extracted from the Snake3D project, this package provides:

- Forward auto-movement with touch steering and lane clamping
- Text-based level generation across 3 lanes
- Head/body collision dispatch (`ICollideWithMouth`, `ICollideWithPart`)
- Collectibles, obstacles, color changers, finish-line multipliers
- Event-driven integration (`RunnerEvents`) so host games keep their own UI, save, and ads

## Requirements

| Dependency | How to install |
|------------|----------------|
| Unity 2022.2+ | — |
| URP (recommended) | Unity Package Manager |
| DOTween | Asset Store or vendored `DOTween.dll` in project |
| TextMeshPro | Included via `package.json` |

Optional: vibration plugin (host implements `IVibrationProvider`).

## Installation

### 1. Place the package folder

Keep the package **outside** your game project (sibling folder recommended):

```
Game App/
  hyper-casual-runner/     ← this package
  Snake3D/                 ← consumer project
  MyNewRunnerGame/
```

### 2. Add to `Packages/manifest.json`

```json
{
  "dependencies": {
    "com.kamyab.hyper-casual-runner": "file:../hyper-casual-runner"
  }
}
```

Adjust the relative path for each consumer project.

### 3. Install DOTween

Import DOTween into the consumer project (`Assets/Plugins/Demigiant/DOTween/`). The package assembly references `DOTween.dll`.

### 4. Import sample (optional)

In Unity: **Window → Package Manager → Hyper Casual Runner Engine → Samples → Import** next to **Basic Runner**.

## Quick Start (new game)

### Step 1 — Add package to manifest

See Installation above.

### Step 2 — Create config assets

In the Project window:

- **Create → Hyper Casual Runner → Engine Settings**
- **Create → Hyper Casual Runner → Runner Events**

Assign prefabs and lane spacing on `RunnerEngineSettings`.

### Step 3 — Add level data

Create `Assets/Resources/Levels/` and add numbered text files (`0.txt`, `1.txt`, …). Set `levelResourcePath` to `Levels/` on the settings asset (default).

### Step 4 — Scene setup

Add to your game scene:

| Component | Purpose |
|-----------|---------|
| `RunnerLevelManager` | Loads level text files |
| `RunnerLevelGenerator` | Spawns track from current level |
| `RunnerGameController` | Run state, tap-to-start, scoring |
| `RunnerColorProvider` | Maps `ColorPicker` enum to Unity colors |
| `RunnerCameraController` | Follow camera |
| Your player with `IRunnerPlayer` | Character logic |

Wire `RunnerGameController` with your `RunnerEngineSettings` and `RunnerEvents` assets.

### Step 5 — Implement your character

```csharp
using HyperCasualRunner;
using UnityEngine;

public class BallRunnerPlayer : MonoBehaviour, IRunnerPlayer
{
  // Implement IRunnerPlayer — movement helpers: RunnerMotor + RunnerInput
}
```

Register at runtime:

```csharp
RunnerContext.Player = myPlayer;
```

Subscribe to `RunnerEvents` for HUD, save, and scene transitions.

## Architecture

```
Host Project                          hyper-casual-runner package
─────────────────────────────────────────────────────────────────
YourPlayer (IRunnerPlayer)     ←→    RunnerContext
Your HUD / Save / Ads          ←→    RunnerEvents (ScriptableObject)
Your prefabs                   →     RunnerEngineSettings
                                     RunnerGameController
                                     RunnerLevelGenerator
                                     RunnerMotor / RunnerInput
```

Snake3D uses thin subclasses (`GameController : RunnerGameController`, `Food : RunnerFood`) so existing prefabs and scenes keep working.

## Level format reference

Each line: `<settings>-<lane content>`

- First line settings char = starting player color: `G` Green, `B` Blue, `R` Red, `P` Purple
- Lane content = pairs of `(char)(digit)`:

| Char | Meaning |
|------|---------|
| `H` + digit | Empty gap (count) |
| `E` + digit | Coins on ground |
| `F` + digit | Coins in air |
| `A` + `0`/`1` | Horizontal obstacle (hidden/visible) |
| `D` + digit | Vertical obstacle (digit ignored) |
| `C` + color char | Color changer |
| `G/B/R/P` + digit | Colored food (count) |

Example (`0.txt`):

```
G-H9G9G9G9G8H9H9H9H5A1F9E5H5H5D1E9E5H5H5
G-H9H9H9G9G8H9H9H9G5A0F9F5H5H5D1E9E5H5H5
G-H9H9G9G9G8H9H9H9H5A1E9F5H5H5D1E9E5H5H5
```

Validate levels: **Tools → Hyper Casual Runner → Validate Levels**

## Events API (`RunnerEvents`)

| Event | When |
|-------|------|
| `onRunStarted` | First tap |
| `onFoodCollected(int count)` | Correct food eaten |
| `onFoodDecreased(int count)` | Wrong food / body hit obstacle |
| `onCoinCollectedAtPosition(Vector3)` | Coin picked up |
| `onRunFailed` | Player died |
| `onRunCompleted(RunResult)` | Level finished |
| `onWrongCollectible` | Wrong color food |
| `onRequestNextLevel` | Host should load next scene |
| `onMultiplierSelected` | Finish sector crossed |

`RunResult` fields: `foodScore`, `coinsCollected`, `multiplier`, `totalEarned`.

## Extending

- **Disable color matching:** set `enableColorMatching = false` on `RunnerEngineSettings`
- **Custom growth:** implement `IRunnerPlayer.Grow()` / `Shrink()` on your character
- **Custom HUD:** listen to `RunnerEvents`; do not modify package scripts
- **Formation switching:** enable `enableFormationSwitch`, handle swipes via `RunnerInput`

## Migration from Snake3D (class mapping)

| Snake3D (host wrapper) | Package base |
|------------------------|--------------|
| `GameController` | `RunnerGameController` |
| `LevelManager` | `RunnerLevelManager` |
| `LevelGenerator` | `RunnerLevelGenerator` |
| `SnakeMotor` | `RunnerMotor` |
| `SnakeInput` | `RunnerInput` |
| `Food` | `RunnerFood` |
| `Coin` | `RunnerCoin` |
| `Obstacle` | `RunnerObstacle` |
| `FinishLine` | `RunnerFinishLine` |
| `CameraController` | `RunnerCameraController` |
| `Snake` | implements `IRunnerPlayer` |

Host wiring: `RunnerBootstrap`, `RunnerEventBridge`.

## Folder structure

```
hyper-casual-runner/
  package.json
  README.md
  CHANGELOG.md
  Runtime/
    HyperCasualRunner.asmdef
    Core/          IRunnerPlayer, RunnerGameController, RunnerEvents, settings
    Movement/      RunnerMotor, RunnerInput
    Level/         RunnerLevelManager, RunnerLevelGenerator
    Collision/     ICollideWithMouth, ICollideWithPart, RunnerHeadTrigger
    World/         Food, coins, obstacles, finish line
    Camera/        RunnerCameraController
  Editor/
    LevelValidator.cs
  Samples~/
    BasicRunner/
```

## License

Use within your studio projects. Third-party deps (DOTween, etc.) remain under their own licenses.
