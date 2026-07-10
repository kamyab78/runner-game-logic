# Test Consumer Template

Use this folder as a checklist when linking the package into a **second** Unity project.

## 1. Create a new Unity 2022.2+ URP project

## 2. Copy or symlink the package

Place `hyper-casual-runner` as a sibling folder:

```
MyStudio/
  hyper-casual-runner/
  MyNewRunnerGame/
```

## 3. Edit `Packages/manifest.json`

```json
{
  "dependencies": {
    "com.kamyab.hyper-casual-runner": "file:../hyper-casual-runner",
    "com.unity.render-pipelines.universal": "14.0.7",
    "com.unity.textmeshpro": "3.0.6"
  }
}
```

## 4. Install DOTween

Import DOTween into `Assets/Plugins/Demigiant/DOTween/`.

## 5. Verify import

Open Unity. Confirm **Package Manager** lists **Hyper Casual Runner Engine** without errors.

## 6. Import sample

Package Manager → Samples → **Basic Runner** → Import.

## 7. Smoke test

- Create `Resources/Levels/0.txt` with one lane line
- Add scene objects per `Samples~/BasicRunner/README.md`
- Enter Play Mode; tap to start; player should move forward

## Validation script

From repo root:

```bash
python3 -c "import json; json.load(open('package.json')); print('package.json OK')"
ls Runtime/*.asmdef Editor/*.asmdef
```
