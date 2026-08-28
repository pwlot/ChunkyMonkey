# Changelog

## Unreleased

## 0.9.5 - 2026-08-09

### Highlights

- Changed ChunkyMonkey distribution to a proprietary software license that permits normal use while prohibiting redistribution, resale, modification, reverse engineering, license bypass, and derivative use.
- Added installer license acceptance and release checks so desktop, CLI, npm, Python, site, and public downloads carry the same first-party terms while preserving third-party licenses.

## 0.9.4 - 2026-08-09

### Highlights

- Added a dedicated Commit action, clearer Stage and Discard controls, and central result views for previews, status, history, comparisons, reports, and errors.
- Replaced GitHub device-code account setup with direct browser authorization.
- Unified Browser Git, GitHub CLI, SSH, and saved identities into one account row per GitHub username.
- Routed clone, pull, push, and supported API operations through the selected account without changing global Git settings.
- Kept repository tabs account-scoped so newly added accounts start empty and returning accounts restore only their own tabs.

## 0.9.3 - 2026-08-04

### Highlights

- Locked the active repository during file, selected, all-file, hunk, and line Stage/Unstage actions until the mutation and refresh finish.
- Updated the free Unity Editor package to `0.1.5` with standalone Git/LFS checks and a bundled numbered PDF manual.
- Clarified comparison descriptions while keeping the full price-and-feature matrix compact.

## 0.9.2 - 2026-08-03

### Highlights

- Made multiple simultaneous repository tabs available in Free across normal open, scan, and open-all workflows.
- Focused lifetime Pro on keeping multiple GitHub identities ready for fast switching.
- Locked a repository during Commit and Push while keeping other open repositories usable.
- Fixed Hugging Face CLI discovery from user Python Scripts and compacted repository toolbar labels.
- Rebuilt the site comparison with clear feature states and 1-year, 5-year, and 10-year ownership-cost views.

## 0.9.1 - 2026-08-01

### Highlights

- Added a built-in Unreal Engine adapter for generated-folder, large-asset, and Git LFS checks.
- Added supported-client setup plus commit-message and code-review prompts to MCP Agent.
- Reworked the client comparison for easier feature and price checks.
- Added local on-device activation as a visible Pro benefit.

## 0.9.0 - 2026-08-01

### Highlights

- Added guarded detection and repair for oversized unpushed Git history, including large assets that should be migrated to Git LFS.
- Made long pushes resilient to active slow transfers, with clearer phase-aware progress and numbered chunk commits.
- Unified toolbar and Commit-panel pushes around the configured chunk target, with a persistent 1000 MB app default.
- Hardened selected-account routing so the identity shown in the header matches the verified SSH push route.
- Added explicit confirmation for push-route changes and destructive branch deletion.
- Improved GitHub and Hugging Face device authentication with visible codes and direct copy/open controls.
- Made staging failures explicit, disabled unsafe commit actions when file state is unavailable, and gave active file lists more panel space.
- Added responsive checks for common Windows 125% and 150% scaled layouts.
- Added clear Terms, Refund Policy, and Privacy sections plus a direct Windows-installer download path.
- Refreshed account discovery, repository controls, right-panel layout, and public download guidance.

## 0.8.9 - 2026-07-27

- Made startup recovery more reliable and added confirmation that an updated frontend loaded successfully.
- Preserved exact staged content, including large files and Git LFS pointers, when committing in chunks.
- Reduced background repository work and improved refresh behavior after external changes.
- Rechecked trusted local add-ons when their code changes.
- Cleaned stale update files automatically after successful installs.
- Added editable per-account commit identity emails and clearer account controls in Settings.
- Added a new-branch button beside the branch selector with guarded create and checkout options.
- Fixed commit-history search and made clearing a search refresh immediately.
- Simplified Health navigation, moved completed reports into the central workspace, and separated reports from repo-changing actions.
- Added browser-based Hugging Face login with visible progress, automatic account refresh, and an older-CLI fallback.
- Added nonzero ahead/behind counts directly to Pull and Push while removing duplicate sidebar status boxes.
- Hardened Git action safety, updater recovery, and add-on trust checks.
- Clarified that Pro is a one-time lifetime purchase with no subscription.

## 0.8.8 - 2026-05-24

- Added Ctrl/Meta and Shift multi-select for staged and unstaged file lists, with selected-file stage, unstage, discard, stash, ignore, and patch-copy actions.
- Added discard-all beside Stage all and confirmed per-file discard from the changed-file context menu.
- Added configured local repo dirty-state refresh polling, plus forced refresh when the app regains focus.
- Expanded changed and committed file context menus with file history, blame, restore/open/reveal/copy/patch actions, and external diff/editor/default-app handoff.
- Fixed file history and blame actions so they open real review panes instead of completing with unusable output text.
- Fixed unstaged discard for mixed staged-and-unstaged files so staged changes are preserved.
- Fixed deleted-file commit blame and restore by reading the file from the selected commit's parent when needed.

## 0.8.7 - 2026-05-21

- Added side-by-side Diff View hunk and zero-context line-block stage/unstage/discard.
- Added left Navigator Pull Requests, Issues, PR risk, and undo/checkpoint groups.
- Added PR risk checks against the target branch, including overlapping files and likely conflict candidates.
- Expanded Merge Doctor with base/ours/theirs/result panes and Git mergetool launch.
- Added footer update status/check button for the selected Live/Dev channel.
- Hardened HF Bucket sync failure copy when the installed HF CLI lacks `hf buckets sync`.
- Fixed the Commit panel action button so push-only states show existing local commits and mixed commit+push states count all commits sent to upstream.
- Added the built-in MCP Agent add-on and `chunkymonkey mcp` server for local AI agents.
- Wired optional AI assistance for supported cloud, local, and compatible custom endpoints.
- Included Windows and experimental Linux x64 release artifacts.

## 0.8.6 - 2026-05-17

- Added Review Prep for checkpointed review branches, deterministic change buckets, push, compare, and PR handoff workflows.
- Added built-in Hugging Face and Unity add-ons for repo-specific Git/LFS checks.
- Added the local add-on contract for folder-based repo adapters with explicit trust before backend code runs.
- Added the free Unity Editor extension source and public Hugging Face Hub repo checker.
- Polished Settings, account/add-on menus, update UI, and dark-mode contrast.
- Hardened updater lifecycle, GitHub auth-route diagnostics, add-on execution, and public release publishing.
- Updated user docs for desktop workflows, Review Prep, Hugging Face, Unity, and add-on developers.

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
