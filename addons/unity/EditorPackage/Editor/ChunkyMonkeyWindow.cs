using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ChunkyMonkey.Unity
{
    public sealed class ChunkyMonkeyWindow : EditorWindow
    {
        private Vector2 scroll;
        private Report report;
        private BridgeStatus bridge;
        private string bridgeMessage;
        private Texture2D logo;
        private bool showDetails;
        private bool showMeta;
        private bool showGenerated;
        private bool showIgnore;
        private bool showLfs;
        private bool showWarnings;
        private bool showLarge;

        [MenuItem("Tools/ChunkyMonkey/Repo Doctor")]
        public static void Open()
        {
            GetWindow<ChunkyMonkeyWindow>("ChunkyMonkey");
        }

        private void OnEnable()
        {
            logo = LoadLogo();
            titleContent = new GUIContent("ChunkyMonkey", logo);
            Refresh();
        }

        private void OnGUI()
        {
            if (report == null) Refresh();
            if (report == null)
            {
                EditorGUILayout.HelpBox("ChunkyMonkey could not scan this Unity project.", MessageType.Warning);
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawHeader();
            EditorGUILayout.Space(8);
            DrawOverview();
            EditorGUILayout.Space(8);
            DrawActions();
            EditorGUILayout.Space(8);
            DrawDetails();
            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            using (new EditorGUILayout.HorizontalScope())
            {
                if (logo != null)
                {
                    GUILayout.Label(logo, GUILayout.Width(54), GUILayout.Height(54));
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField("ChunkyMonkey Unity Tools", Styles.Header);
                    EditorGUILayout.LabelField("Repo Doctor", EditorStyles.miniBoldLabel);
                    EditorGUILayout.LabelField(ProjectName(), EditorStyles.miniLabel);
                }
            }
        }

        private void DrawOverview()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField(report.HasWarnings ? "Action needed" : "Project looks clean", Styles.Header);
                EditorGUILayout.LabelField(SummaryText(), EditorStyles.wordWrappedMiniLabel);
                EditorGUILayout.Space(6);

                DrawMetricRow(
                    ("Missing .meta", report.MissingMeta.Count),
                    ("Generated folders", report.GeneratedFolders.Count),
                    ("Missing LFS rules", report.MissingLfsRules.Count));

                DrawMetricRow(
                    ("Git ignore gaps", report.MissingIgnoreRules.Count),
                    ("Large assets", report.LargeUntrackedAssets.Count),
                    ("Scan warnings", report.ScanWarnings.Count));
            }
        }

        private void DrawActions()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawBridgeStatus();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Refresh", GUILayout.Width(110), GUILayout.Height(28))) Refresh();
                }

                EditorGUILayout.Space(6);
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(report.MissingIgnoreRules.Count == 0))
                    {
                        if (ToolButton("Apply .gitignore")) ApplyGitIgnore();
                    }

                    using (new EditorGUI.DisabledScope(report.MissingLfsRules.Count == 0))
                    {
                        if (ToolButton("Apply LFS Rules")) ApplyLfsRules();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(!bridge.Found))
                    {
                        if (ToolButton("Open in ChunkyMonkey")) OpenChunkyMonkey();
                    }

                    if (ToolButton("Download ChunkyMonkey")) DownloadChunkyMonkey();
                }

                if (!string.IsNullOrWhiteSpace(bridgeMessage))
                {
                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField(bridgeMessage, EditorStyles.wordWrappedMiniLabel);
                }
            }
        }

        private void DrawDetails()
        {
            showDetails = EditorGUILayout.Foldout(showDetails, "Details", true);
            if (!showDetails) return;

            DrawSection("Missing .meta files", report.MissingMeta, ref showMeta);
            DrawSection("Generated folders present", report.GeneratedFolders, ref showGenerated);
            DrawSection("Missing .gitignore rules", report.MissingIgnoreRules, ref showIgnore);
            DrawSection("Missing .gitattributes LFS rules", report.MissingLfsRules, ref showLfs);
            DrawSection("Scan warnings", report.ScanWarnings, ref showWarnings);
            DrawSection("Large untracked assets", report.LargeUntrackedAssets, ref showLarge);
        }

        private void DrawBridgeStatus()
        {
            var status = bridge.Found ? "Desktop app: Installed" : "Desktop app: Not installed";
            var style = bridge.Found ? EditorStyles.miniBoldLabel : EditorStyles.miniLabel;
            EditorGUILayout.LabelField(status, style, GUILayout.MinWidth(180));
        }

        private static void DrawMetricRow((string Label, int Count) a, (string Label, int Count) b, (string Label, int Count) c)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawMetric(a.Label, a.Count);
                DrawMetric(b.Label, b.Count);
                DrawMetric(c.Label, c.Count);
            }
        }

        private static void DrawMetric(string label, int count)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.MinWidth(120), GUILayout.Height(58)))
            {
                EditorGUILayout.LabelField(count.ToString(), CountStyle(count));
                EditorGUILayout.LabelField(label, EditorStyles.miniLabel);
            }
        }

        private static void DrawSection(string title, IReadOnlyList<string> rows, ref bool expanded)
        {
            EditorGUILayout.Space(6);
            expanded = EditorGUILayout.Foldout(expanded, $"{title} ({rows.Count})", true);
            if (!expanded) return;

            if (rows.Count == 0)
            {
                EditorGUILayout.LabelField("OK", EditorStyles.miniLabel);
                return;
            }

            foreach (var row in rows.Take(80))
            {
                EditorGUILayout.SelectableLabel(row, EditorStyles.miniLabel, GUILayout.Height(18));
            }

            if (rows.Count > 80)
            {
                EditorGUILayout.LabelField($"+ {rows.Count - 80} more", EditorStyles.miniLabel);
            }
        }

        private void Refresh()
        {
            try
            {
                report = RepoScanner.Scan(ProjectRoot());
                bridge = ChunkyMonkeyBridge.Detect();
                bridgeMessage = string.Empty;
            }
            catch (Exception error)
            {
                report = new Report
                {
                    ProjectRoot = ProjectRoot(),
                    ScanWarnings = new List<string> { error.Message }
                };
                bridge = BridgeStatus.NotFound();
                bridgeMessage = string.Empty;
            }

            Repaint();
        }

        private void ApplyGitIgnore()
        {
            FileRules.AppendMissing(Path.Combine(ProjectRoot(), ".gitignore"), ChunkyMonkeyWindowRules.IgnoreRules);
            AssetDatabase.Refresh();
            Refresh();
        }

        private void ApplyLfsRules()
        {
            FileRules.AppendMissing(
                Path.Combine(ProjectRoot(), ".gitattributes"),
                ChunkyMonkeyWindowRules.LfsPatterns.Select(pattern => $"{pattern} filter=lfs diff=lfs merge=lfs -text"));
            AssetDatabase.Refresh();
            Refresh();
        }

        private void OpenChunkyMonkey()
        {
            var result = ChunkyMonkeyBridge.Open(bridge, ProjectRoot());
            bridgeMessage = result.Ok ? "Opened this project in ChunkyMonkey." : result.Output;
            Repaint();
        }

        private static void DownloadChunkyMonkey()
        {
            Application.OpenURL("https://chunkymonkey.dev/#download");
        }

        private string SummaryText()
        {
            if (!report.IsGitRepo) return "This project is not inside a Git repository yet.";
            if (!report.HasWarnings) return "Git, Unity generated folders, .meta files, and LFS rules look ready.";
            return $"{report.IssueCount} repo hygiene item(s) found. Apply safe rules here, then review file-level details only when needed.";
        }

        private string ProjectName()
        {
            var root = ProjectRoot();
            return string.IsNullOrWhiteSpace(root) ? "Unity project" : new DirectoryInfo(root).Name;
        }

        private static string ProjectRoot()
        {
            return Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
        }

        private static bool ToolButton(string label)
        {
            return GUILayout.Button(label, GUILayout.Height(30));
        }

        private static GUIStyle CountStyle(int count)
        {
            return count == 0 ? Styles.CountOk : Styles.CountWarning;
        }

        private static Texture2D LoadLogo()
        {
            var directPaths = new[]
            {
                "Packages/com.pwlot.chunkymonkey-unity/Images/chunkymonkey-biting-icon.png",
                "Assets/ChunkyMonkey/Images/chunkymonkey-biting-icon.png"
            };

            foreach (var path in directPaths)
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (texture != null) return texture;
            }

            var guids = AssetDatabase.FindAssets("chunkymonkey-biting-icon t:Texture2D");
            return guids.Length == 0 ? null : AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        private static class Styles
        {
            public static readonly GUIStyle Header = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16
            };

            public static readonly GUIStyle CountOk = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 20,
                normal = { textColor = new Color(0.48f, 0.72f, 0.48f) }
            };

            public static readonly GUIStyle CountWarning = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 20,
                normal = { textColor = new Color(1f, 0.68f, 0.18f) }
            };
        }
    }
}
