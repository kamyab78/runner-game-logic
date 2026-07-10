# Basic Runner Sample

Import via **Package Manager → Hyper Casual Runner Engine → Samples**.

## What it includes

- `PlaceholderRunnerPlayer.cs` — minimal `IRunnerPlayer` implementation

## Setup after import

1. Create `RunnerEngineSettings` and `RunnerEvents` assets
2. Add `RunnerLevelManager`, `RunnerLevelGenerator`, `RunnerGameController`, `RunnerColorProvider` to a scene
3. Add a GameObject with `PlaceholderRunnerPlayer`, `RunnerMotor`, `RunnerInput`, and a head trigger collider (`RunnerHeadTrigger` on child)
4. Add level files under `Resources/Levels/`
5. Wire `RunnerEvents` to your HUD scripts

See the main [README](../../README.md) for full documentation.
