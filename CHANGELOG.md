# Changelog

## Unreleased

## 0.8.6 - 2026-05-17

- Added Review Prep for checkpointed review branches, deterministic change buckets, push, compare, and PR handoff workflows.
- Added built-in Hugging Face and Unity add-ons for repo-specific Git/LFS checks.
- Added the local add-on contract for folder-based repo adapters with explicit trust before backend code runs.
- Added the free Unity Editor extension source and public Hugging Face Hub repo checker.
- Polished Settings, account/add-on menus, update UI, and dark-mode contrast.
- Hardened updater lifecycle, GitHub auth-route diagnostics, add-on execution, and public release publishing.
- Updated public docs for desktop workflows, Review Prep, Hugging Face, Unity, and add-on developers.

## 0.8.5 - 2026-05-15

- Hardened staged commit chunking for added, modified, deleted, and renamed files.
- Fixed push-only flows when a clean repo has local commits ahead of upstream.
- Improved repo refresh behavior after external Git changes and completed pushes.
- Added clearer account, status, and output panel close behavior.
- Added hidden-parent hints for truncated history graph views.
- Polished Git progress and status progress bar direction.

## 0.8.4 - 2026-05-14

- Polished progress bar direction across status, update, and Git progress views.

## 0.8.3 - 2026-05-14

- Fixed status and output panels reopening during background repo and update refreshes.

## 0.8.2 - 2026-05-14

- Improved status handling across history, background jobs, and update checks.
- More reliable automated Git actions and Live/Dev updates.

## 0.8.1 - 2026-05-14

- Fixed commit history loading for repos whose commit messages contain words like `error` or `failed`.

## 0.8.0 - 2026-05-13

- Made new branch creation explicit opt-in; commits now default to the current branch.
- Sanitized new branch names only when branch creation is enabled.
- Cleared and ignored stale branch text when branch creation is off.
- Added automatic upstream setup when pushing a newly created branch.
- Cleaned up commit graph rendering so lanes do not draw dangling stubs.

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
