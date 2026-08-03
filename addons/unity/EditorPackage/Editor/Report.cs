using System.Collections.Generic;

namespace ChunkyMonkey.GitLfsTools
{
    internal sealed class Report
    {
        public string ProjectRoot;
        public bool IsGitRepo;
        public List<string> MissingMeta = new List<string>();
        public List<string> GeneratedFolders = new List<string>();
        public List<string> MissingIgnoreRules = new List<string>();
        public List<string> MissingLfsRules = new List<string>();
        public List<string> ScanWarnings = new List<string>();
        public List<string> LargeAssets = new List<string>();

        public int IssueCount
        {
            get
            {
                return MissingMeta.Count +
                    GeneratedFolders.Count +
                    MissingIgnoreRules.Count +
                    MissingLfsRules.Count +
                    ScanWarnings.Count +
                    LargeAssets.Count;
            }
        }

        public bool HasWarnings
        {
            get { return IssueCount > 0; }
        }
    }
}
