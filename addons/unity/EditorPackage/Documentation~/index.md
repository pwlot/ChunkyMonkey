# ChunkyMonkey Git and LFS Tools

ChunkyMonkey Git and LFS Tools is an Editor-only repository checker for projects that use Git and Git LFS. It works without accounts, network access, external services, or bundled executables.

For the complete beginner guide, open `ChunkyMonkey-Git-and-LFS-Tools-Manual.pdf` in this folder.

## Table of Contents

1. [Requirements](#1-requirements)
2. [Installation](#2-installation)
3. [Open Repo Doctor](#3-open-repo-doctor)
4. [Understand the Checks](#4-understand-the-checks)
5. [Apply Repository Rules](#5-apply-repository-rules)
6. [Limitations](#6-limitations)
7. [Troubleshooting](#7-troubleshooting)
8. [Technical Reference](#8-technical-reference)

## 1. Requirements

1. Unity Editor 6000.0.40f1 or newer.
2. A project that uses, or will use, Git version control.
3. Git LFS if you want the generated LFS rules to affect file tracking.

No additional Unity packages are required. The tool does not install or run Git or Git LFS.

## 2. Installation

1. Download the package from `Window > Package Manager > My Assets`.
2. Import the complete `Assets/ChunkyMonkey` folder.
3. Wait for script compilation to finish.
4. Confirm that the Console contains no package errors.

## 3. Open Repo Doctor

1. Open your project.
2. Select `Tools > ChunkyMonkey > Repo Doctor`.
3. Wait for the first scan to finish.
4. Read the overview counts before applying changes.

## 4. Understand the Checks

1. `Missing .meta` lists files or folders under `Assets/` that do not have a matching `.meta` file. The tool reports the problem but does not create the missing files.
2. `Generated folders` reports `Library`, `Temp`, `Obj`, `Build`, `Builds`, `Logs`, and `UserSettings` folders found at the project root.
3. `Git ignore gaps` lists recommended generated-folder rules missing from the root `.gitignore` file.
4. `Missing LFS rules` lists recommended Git LFS rules missing from the root `.gitattributes` file.
5. `Large assets` lists files under `Assets/` that are at least 50 MB.
6. `Scan warnings` explains any folder or file that could not be checked.

Use the `Details` foldout to see individual paths and rules. Lists are intentionally capped to keep the Editor responsive.

## 5. Apply Repository Rules

1. Commit or back up `.gitignore` and `.gitattributes` before changing them.
2. Click `Apply .gitignore` to append missing generated-folder rules.
3. Click `Apply LFS Rules` to append missing LFS rules for common binary asset extensions.
4. Review both files in your version-control client.
5. Click `Refresh` to rerun every check.

Existing file contents are preserved. The tool appends missing lines only. Adding LFS rules does not migrate files that were already committed to Git history.

## 6. Limitations

1. Missing `.meta` files are reported but not generated.
2. Generated folders are reported but not deleted.
3. The large asset threshold is fixed at 50 MB.
4. Detail lists show up to 200 missing `.meta` entries and 100 large assets.
5. The tool does not commit, push, upload, migrate history, or modify Project Settings.
6. Repository files change only after an explicit button click.

## 7. Troubleshooting

1. If the menu is missing, wait for compilation and check the Console for errors.
2. If the project is reported as not inside Git, confirm a `.git` directory exists at the project root.
3. If a rule still appears missing, check the spelling and location of the root `.gitignore` or `.gitattributes` file, then click `Refresh`.
4. If a scan warning appears, expand `Details > Scan warnings` and check file permissions for the listed path.
5. If LFS rules are present but files are not tracked, install and configure Git LFS outside the Editor, then follow normal Git LFS migration guidance for previously committed files.

## 8. Technical Reference

1. Editor-only C# source code in a dedicated assembly definition.
2. No runtime components, scenes, prefabs, shaders, native plugins, executables, network calls, telemetry, or external services.
3. No third-party packages or third-party code.
4. Compatible with Built-in, Universal, and High Definition render pipelines because the package does not alter rendering.
5. Supported on Windows, macOS, and Linux Editors supported by Unity 6000.0.40f1 or newer.
