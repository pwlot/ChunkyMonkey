# ChunkyMonkey Git and LFS Tools

Editor-only repository checks for projects that use Git and Git LFS.

After importing the package, open `Tools > ChunkyMonkey > Repo Doctor`.

The tool checks:

1. Missing `.meta` files under `Assets/`.
2. Generated project folders that should normally be ignored.
3. Missing `.gitignore` rules.
4. Missing `.gitattributes` rules for common binary asset types.
5. Project assets that are 50 MB or larger.

Use `Apply .gitignore` or `Apply LFS Rules` to append only missing rules. Existing file contents are preserved.

Start with `Documentation/ChunkyMonkey-Git-and-LFS-Tools-Manual.pdf` for installation, usage, limitations, and troubleshooting.
