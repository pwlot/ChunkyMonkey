# ChunkyMonkey

<p align="center">
  <img src="assets/chunkymonkey-biting-logo.webp" alt="ChunkyMonkey logo" width="420">
</p>

ChunkyMonkey is a Git/LFS desktop app and CLI for large game, ML, media, and research repos.

Official site: [chunkymonkey.dev](https://chunkymonkey.dev)

Docs: [chunkymonkey.dev/docs](https://chunkymonkey.dev/docs)

I built it after dealing with huge pushes, Git LFS mistakes, merge conflicts, timeouts, and unreliable connections in game and ML projects. It splits large commits into practical chunks, catches LFS problems before they hurt, and gives conflict recovery a safer local workflow.

## Release

[Download the latest Windows installer: `ChunkyMonkeySetup.exe`](https://github.com/pwlot/ChunkyMonkey/releases/latest/download/ChunkyMonkeySetup.exe)

ChunkyMonkey currently ships for Windows only. The installer includes the desktop app and CLI. You can also download the [standalone Windows CLI](https://github.com/pwlot/ChunkyMonkey/releases/latest/download/chunkymonkey-cli-windows-x64.zip) and [SHA-256 checksums](https://github.com/pwlot/ChunkyMonkey/releases/latest/download/checksums.txt). Windows may show an unknown-publisher or SmartScreen warning.

## What it does

- Auto-chunks large commits and pushes into smaller parts.
- Helps avoid push timeouts on slow or unreliable connections.
- Checks Git LFS coverage for large assets, model files, datasets, video, audio, and binaries.
- Shows push/pull progress, speed, ETA, and failure status.
- Includes Diff View for side-by-side file review with whitespace-noise hiding plus hunk and line-block stage/unstage/discard.
- Includes Merge Doctor for conflicted files: base/ours/theirs/result panes, keep ours, keep theirs, keep both, open an editor, launch mergetool, ask an optional LLM helper, accept a reviewed suggestion, abort, or finish.
- Lets you bring your own key for optional AI assistance through supported cloud and local providers or a compatible custom endpoint.
- Scans folders for Git repos and keeps large workspaces manageable.
- Clones repos, creates repos, and works with GitHub accounts through local Git/GitHub tools.
- Shows commit history and branch state.
- Protects branch checkout when the worktree has changes.
- Includes Review Prep for checkpoints, deterministic change buckets, review branches, PR risk checks, compare links, push, and PR handoff when GitHub CLI is ready.
- Shows GitHub Pull Requests, Issues, PR risk, stashes, conflicts, and undo/checkpoint entries in the left Navigator.
- Includes repo health and repair tools for stale remotes, LFS state, repo bloat, cache folders, and generated files.
- Includes templates and helpers for game, ML, media, and research repos.
- Includes built-in Unity, Unreal Engine, Hugging Face, and MCP Agent add-ons for repo-specific Git/LFS checks and local agent access.
- Lets developers add local repo adapters through a small add-on manifest and Python backend contract.
- Connects compatible MCP clients to commit/review prompts, repo context, and guarded Git actions through `chunkymonkey mcp`.
- Exports diagnostics only when you ask for them.
- Provides both desktop and CLI workflows.
- Remembers the last monitor and keeps update refreshes grouped with the installed app.
- Offers a one-time Pro upgrade for keeping multiple GitHub identities ready for fast switching.

## Why it exists

ChunkyMonkey is for repos where pushes fail because the repo is big, binary-heavy, or sitting behind a bad connection:

- Unity, Unreal, Godot, and custom engine projects.
- ML projects with checkpoints, weights, datasets, notebooks, generated artifacts, and experiment output.
- Media projects with large video, audio, image, cache, and export folders.
- Research repos with many generated files and fragile reproduction state.
- Any repo where a normal push can turn into a timeout, LFS mistake, or cleanup session.

## CLI

The CLI and desktop app use the same core logic, so the same workflow is available from either surface.

Typical CLI use:

```bash
chunkymonkey
# 1. Commit + push chunks
# Chunk target? 500mb
# Parts? 4
# Commit message? Add assets
# Preview first? y
# Push? y
```

ChunkyMonkey uses the current Git repo automatically, picks practical chunk sizes, and pushes in smaller pieces so huge commits are less likely to fail halfway through. Slow connection, large assets, bad LFS setup: those are the cases it is built for.

For automation and scripts:

```bash
chunkymonkey status --repo .
chunkymonkey radar --repo .
chunkymonkey ml-report --repo .
chunkymonkey preview --chunk-size 500mb --parts 2
chunkymonkey commit --chunk-size 500mb --parts 2 --message "Add assets"
```

## Add-ons

Add-ons are local repo adapters. Settings > Add-ons is for enabling included add-ons, adding local folders, trusting local backend code, refreshing status, and running actions.

Built-ins:

<table>
  <tr>
    <td width="50%" valign="top">
      <img src="https://huggingface.co/front/assets/huggingface_logo-noborder.svg" alt="Hugging Face logo" width="42"><br>
      <strong>Hugging Face</strong><br>
      Inspect model, dataset, Space, and Storage Bucket state. Preview bucket sync before writing.
    </td>
    <td width="50%" valign="top">
      <picture>
        <source media="(prefers-color-scheme: dark)" srcset="https://cdn.simpleicons.org/unity/FFFFFF">
        <img src="https://cdn.simpleicons.org/unity/000000" alt="Unity logo" width="42">
      </picture><br>
      <strong>Unity</strong><br>
      Find missing <code>.meta</code> files, generated folders, ignore/LFS gaps, and large project assets. Includes the free Unity Editor extension.
    </td>
  </tr>
  <tr>
    <td width="50%" valign="top">
      <picture>
        <source media="(prefers-color-scheme: dark)" srcset="https://cdn.simpleicons.org/unrealengine/FFFFFF">
        <img src="https://cdn.simpleicons.org/unrealengine/000000" alt="Unreal Engine logo" width="42">
      </picture><br>
      <strong>Unreal Engine</strong><br>
      Find generated folders, large untracked assets, and tracked <code>.uasset</code> or <code>.umap</code> files outside Git LFS.
    </td>
    <td width="50%" valign="top">
      <img src="assets/mcp-agent-robot-monkey-transparent.png" alt="MCP Agent robot monkey" width="46"><br>
      <strong>MCP Agent</strong><br>
      Expose repo status, diffs, history, Review Prep, and confirmation-gated Git actions to compatible local MCP clients.
    </td>
  </tr>
</table>

Install ChunkyMonkey Git and LFS Tools from Unity Package Manager:

```text
https://github.com/pwlot/ChunkyMonkey.git?path=addons/unity/EditorPackage
```

Developer docs: [chunkymonkey.dev/docs/add-ons](https://chunkymonkey.dev/docs/add-ons)

## Release channel

ChunkyMonkey is a public release built around local Git workflows.

This public repo is for:

- downloads
- release notes
- checksums
- bug reports
- support docs
- security/contact info

The source code is not public.

## Trust model

ChunkyMonkey shells out to local Git. It does not host your repos, sync private files to a service, or run background telemetry.

Risky operations are explicit. Bug reports are user-triggered. Diagnostics are exported locally unless you choose to send them.

Discarding a hunk or line block creates a local checkpoint first. Merge Doctor creates local backup sessions before writing a resolved conflict file. It stages only the selected file and does not create a commit automatically. MCP write tools require explicit `confirm=true`. Optional AI assistance uses the service, API key, and optional base URL you configure locally; API keys are kept in the OS credential store when available. If a request fails or returns unsafe output, ChunkyMonkey falls back to deterministic guidance.

Public bug reports omit raw local paths, remotes, account names, and file lists. Use the local diagnostics export when you need to share private detail with someone you trust.

## Source Availability

ChunkyMonkey desktop and CLI are distributed through public release files. The Unity Editor package source is visible under `addons/unity/EditorPackage` for inspection and integration, but it is not open-source software and remains covered by the ChunkyMonkey Software License Agreement.

## License

ChunkyMonkey is proprietary software distributed under the [ChunkyMonkey Software License Agreement](LICENSE). It may be installed and used under that agreement, but it may not be redistributed, resold, modified, reverse engineered, or used to build derivative products except where applicable law requires otherwise. Release packages include a copy of the license.

## Bugs

Use GitHub Issues:

[github.com/pwlot/ChunkyMonkey/issues/new](https://github.com/pwlot/ChunkyMonkey/issues/new)

Do not include secrets, tokens, private repo contents, or proprietary files in public issues.

For security-sensitive reports, email [chunkymonkey@pwlot.com](mailto:chunkymonkey@pwlot.com).

## Support

Support development:

[pwlot.com/#support](https://www.pwlot.com/#support)

Policies: [Terms](https://chunkymonkey.dev/legal#terms) | [Refunds](https://chunkymonkey.dev/legal#refunds) | [Privacy](https://chunkymonkey.dev/legal#privacy)
