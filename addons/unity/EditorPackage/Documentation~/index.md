# ChunkyMonkey Unity Tools

ChunkyMonkey Unity Tools is a free Editor extension for Unity projects that use Git and Git LFS.

Open it from `Tools > ChunkyMonkey > Repo Doctor`.

Install from Unity Package Manager:

```text
https://github.com/pwlot/ChunkyMonkey.git?path=addons/unity/EditorPackage
```

## What It Checks

1. Missing `.meta` files under `Assets/`.
2. Generated folders that should not be committed, including `Library/`, `Temp/`, `Obj/`, `Build/`, `Builds/`, `Logs/`, and `UserSettings/`.
3. Missing Unity `.gitignore` rules.
4. Missing Git LFS rules for common Unity binary assets.
5. Large untracked assets over 50 MB.

## Buttons

1. `Refresh`: reruns the checks.
2. `Apply .gitignore`: appends missing Unity ignore rules.
3. `Apply LFS Rules`: appends missing `.gitattributes` rules.
4. `Open in ChunkyMonkey`: opens the detected ChunkyMonkey desktop app for this project.
5. `Download ChunkyMonkey`: opens the download page.

The extension does not bundle ChunkyMonkey desktop and does not auto-launch external apps. Launch failures stay inside the Editor window.
