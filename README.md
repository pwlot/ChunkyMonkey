# ChunkyMonkey

<p align="center">
  <img src="assets/chunkymonkey-biting-logo.webp" alt="ChunkyMonkey logo" width="420">
</p>

ChunkyMonkey is a desktop Git/LFS app and CLI for game, ML, media, and research repos with large files or many assets.

Official site: https://chunkymonkey-sand.vercel.app

I created this years ago when I got annoyed by LFS tracking issues, timeouts, huge commits, and fragile Git workflows in very large machine-learning and game development projects. It became my go-to tool, mostly as a CLI. I hope people who work in repos full of models, datasets, builds, textures, audio, video, and generated files find some peace using it.

## Release

The public release channel has been reset for hardened signed builds:

https://github.com/pwlot/ChunkyMonkey/releases/latest

Desktop binaries will be attached only after the hardened release pipeline produces signed artifacts. Python wheels, Python source archives, and npm tarballs are not distributed through the public GitHub Release unless source exposure is intentional for that release.

## What it does

- Chunk large commits by file count or target size, with auto sizing for big changes.
- Check Git LFS coverage for large assets, model files, datasets, video, audio, and binary blobs.
- Warn before weird operations, like hundreds of huge chunks or missing LFS coverage.
- Scan folders for Git repos and keep overflow repos in a picker instead of cramming tabs forever.
- Clone repos, create repos, and work with GitHub accounts through local Git/GitHub tooling.
- Show pull/push progress, speed, ETA, and failure status.
- Show a commit history graph.
- Handle branch checkout with dirty-worktree protection.
- Provide repair/health tools for common Git mess: stale remotes, LFS state, fsck, repo bloat, cache folders, generated files.
- Provide Games, ML, and Media repo templates.
- Provide ML helpers for artifact reports, notebook audits, manifest generation, repro snapshots, and junk cleanup.
- Export diagnostics and create bug reports with useful local context.
- Provide a CLI that uses the same Python backend as the desktop app.

## Why it exists

Normal Git clients are good at normal repos. ChunkyMonkey is for repos where the hard part is not writing a commit message, but surviving the size and shape of the repo:

- Unity, Unreal, Godot, and custom engine projects.
- ML projects with checkpoints, weights, datasets, notebooks, generated artifacts, and experiment output.
- Media projects with large video, audio, image, cache, and export folders.
- Research repos with many generated files and fragile reproduction state.
- Any repo where a normal push can turn into a timeout, LFS mistake, or cleanup session.

## CLI

The CLI and desktop app share the same backend. The goal is boring parity: anything important should be scriptable and usable from the GUI.

Typical CLI uses:

```bash
chunkeymonkey
# 1. Commit + push chunks
# Chunk target: file count or MB [500mb]
# Parts/chunks [1]
# Commit message [suggested from changed files]
# Uses the current repo automatically.

chunkymonkey status --repo .
chunkymonkey radar --repo .
chunkymonkey ml-report --repo .
chunkymonkey preview --chunk-size 500mb --parts 2
chunkymonkey commit --chunk-size 500mb --parts 2 --message "Add assets"
```

## Release channel

ChunkyMonkey is currently an alpha release. That means usable, but still changing fast.

The public repo is for:

- downloads
- release notes
- checksums
- update manifests
- bug reports
- support docs
- security/contact info

The source code is not public.

## Trust model

ChunkyMonkey shells out to local Git. It does not host your repos, sync private files to a service, or run background telemetry.

Risky operations are explicit. Bug reports are user-triggered. Diagnostics are exported locally unless you choose to send them.

## Source Availability

ChunkyMonkey is distributed through signed public release artifacts when available. The source code is not public.

## Bugs

Use GitHub Issues:

https://github.com/pwlot/ChunkyMonkey/issues/new

Do not include secrets, tokens, private repo contents, or proprietary files in public issues.

## Support

Support development:

https://www.pwlot.com/#support
