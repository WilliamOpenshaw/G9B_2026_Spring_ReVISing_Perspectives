# Copilot / AI Agent Instructions (project-specific)

- Project type: Unity (C#) game project using Universal Render Pipeline and TextMeshPro. Open in Unity Editor (use ProjectSettings/ProjectVersion.txt to pick matching Unity version).
- Key locations:
  - Assets/Scripts/: main C# scripts. Example: `Assets/Scripts/kolstons_pop.cs` (see common issues below).
  - Assets/*.unity: scenes (open with Unity Editor to run/play).
  - InputSystem_Actions.inputactions: project uses Unity Input System assets.
  - Packages/manifest.json and ProjectSettings/: package and project configuration.

What you should know before editing code
- Unity requires public `MonoBehaviour` classes to be in files with the same class name. If you rename a class, rename its file (or vice versa). Example fix performed: `kolstons_pop` class name must match `kolstons_pop.cs`.
- Avoid duplicate top-level classes or stray text at the top of C# files — Unity/C# compilers will fail. Check for accidental paste or leftover template code.
- Prefer `transform` shorthand over `gameObject.transform` when inside a `MonoBehaviour`.
- Event handlers: IPointerEnterHandler/IPointerExitHandler require `using UnityEngine.EventSystems;` and correct method signatures (`OnPointerEnter(PointerEventData)` / `OnPointerExit(PointerEventData)`).

Code style & small conventions observed in repo
- Scripts live under `Assets/Scripts/` and are simple MonoBehaviours (no namespaces used).
- Public fields are used for simple state (e.g., `public float sizeX`), rely on Unity serialization if preset in inspector.
- Keep changes minimal and focused — this repo values small, surgical edits over large refactors.

Build / test / debug workflow (how to verify changes)
- Most changes must be validated inside Unity Editor: open project in Unity Hub with the version from `ProjectSettings/ProjectVersion.txt`.
- To quickly validate a script fix: open Unity, let it compile, then enter Play mode and exercise the scene that uses the script (Scenes are in `Assets/Scenes` or top-level .unity files).
- There is no automated test harness in the repo — rely on Unity Play mode and console logs. Use `Debug.Log` for runtime checks.

Integration and external dependencies
- Uses Unity packages (see `Packages/manifest.json`) — do not manually edit generated package files unless necessary.
- Uses the new Input System asset `InputSystem_Actions.inputactions` — changes here affect runtime input handling.

When creating a PR or editing files
- Explain why the change is required and how you validated it (Unity Editor compile + Play mode steps).
- Keep edits focused: fix the minimal set of lines needed to resolve compiler/runtime errors.
- If adding a new script, name the file to match the MonoBehaviour class exactly.

Examples of issues to watch for (from repo)
- `Assets/Scripts/kolstons_pop.cs` (example): common symptoms — accidental characters before `using` lines, duplicate class templates, and class/file name mismatch. Fixes usually:
  - Remove stray text or duplicate template code.
  - Ensure a single public MonoBehaviour class per file and that the class name matches the filename.

If unsure
- Open the project in Unity Editor and reproduce the compiler or runtime error. Provide the console output and the file that triggered the error.
- Ask for the Unity version if not present or if compilation behaves unexpectedly.

If you update this file
- Keep it concise and concrete. Add references to any new build or workflow steps you discover.

---
If any section is unclear or you'd like more examples (e.g., more sample fixes or preferred Git/PR conventions), tell me which area to expand.