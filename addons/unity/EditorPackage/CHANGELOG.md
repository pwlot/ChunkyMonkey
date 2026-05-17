# Changelog

## 0.1.4

- Reworked the Repo Doctor window for clean store screenshots and day-to-day use.
- Hid local filesystem paths from the default UI.
- Split scan, rule, process, and report logic into smaller editor modules.

## 0.1.3

- Added macOS/Linux desktop bridge path detection and `CHUNKYMONKEY_DESKTOP` override.
- Added release/CI guardrails so the public Git package copy cannot drift from the source package.

## 0.1.2

- Added branded header with the ChunkyMonkey monkey icon.
- Cleaned up the desktop bridge panel and action layout.

## 0.1.1

- Added ChunkyMonkey desktop install status.
- Added explicit download button.
- Changed desktop launch to use detected executable paths without shell prompts.
- Kept launch failures inside the Editor window instead of opening fallback prompts.

## 0.1.0

- Added Unity repo doctor Editor window.
- Added missing `.meta` scanner.
- Added generated-folder checks.
- Added `.gitignore` and `.gitattributes` rule checks.
- Added apply buttons for Unity ignore and LFS rules.
- Added optional user-click handoff to ChunkyMonkey desktop.
