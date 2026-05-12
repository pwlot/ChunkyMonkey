# Changelog

## Unreleased

## 0.7.2 - 2026-05-12

- Fixed slow chunk push progress and recovery behavior.
- Fixed Windows update staging/install flow.
- Added Live/Dev update channels with clean generated dev build versions.
- Improved settings layout, update controls, dark mode switches, and repo picker controls.
- Removed account reorder arrow buttons; drag reorder remains.
- Simplified public docs into one compact guide.
- Windows release artifacts remain unsigned until signing credentials are configured.

## 0.7.1 - 2026-05-10

- Added Linux desktop release artifacts: `.deb` and `tar.gz`.
- Added standalone Linux CLI release artifact.
- Kept Windows installer and Windows CLI artifacts available.
- Updated downloads to route through the latest GitHub Release.
- macOS remains planned, but is not included in this release.
- Windows SmartScreen may warn on early installer builds while signing and reputation settle.

## 0.7.0 - 2026-05-10

- Added ChunkyMonkey Pro with signed lifetime license activation.
- Added Stripe checkout, license success page, and branded purchase email.
- Added launch promo support for checkout.
- Added multi-repo workspace tools, fast account menus, and profile workflows.
- Added repo management for known repos, opening from disk, opening all repos in a folder, repo creation, search, and browsing.
- Added GitHub account/org refresh through local GitHub CLI state.
- Added default editor detection and open-repo-in-editor support.
- Added slim output/run dock with CLI command support.
- Improved branch controls, tab overflow, repo context menus, staged/unstaged file scrolling, and history graph layout.
- Improved human-readable errors and Unicode commit/history rendering.
- Added user docs for the desktop app, CLI, Pro, settings, repo management, and troubleshooting.
- No telemetry. Diagnostics and bug reports are user-triggered.
- Source is not public yet.
- Windows SmartScreen may warn on early installer builds while signing and reputation settle.

## 0.1.3 - 2026-05-05

- Added Windows installer download: `ChunkyMonkeySetup.exe`.
- Added standalone Windows CLI download: `chunkymonkey-cli-windows-x64.zip`.
- Added interactive `chunkymonkey` menu for chunked commits and pushes from the current repo.
- Added chunked push flow for large repos, LFS-heavy projects, and slower connections.
- Added SHA-256 checksums for public downloads.
- No telemetry. Diagnostics and bug reports are user-triggered.
- Source is not public yet.
- Windows SmartScreen may warn on early installer builds while signing and reputation settle.
