# Changelog

## [1.0.0] - 2026-07-10

### Added
- Initial UPM package extraction from Snake3D
- `IRunnerPlayer` abstraction for swappable characters
- `RunnerEvents` ScriptableObject for decoupled UI/save integration
- `RunnerEngineSettings` for prefabs, lane spacing, and feature flags
- `RunnerContext` service locator for runtime wiring
- Core systems: movement, level generation, collision, finish-line scoring
- Editor level validator (`Tools → Hyper Casual Runner → Validate Levels`)
- Basic Runner sample with placeholder player script

### Notes
- Requires DOTween in the consumer project
- Snake3D consumes this package via `file:../hyper-casual-runner`
