namespace ChunkyMonkey.Unity
{
    internal static class ChunkyMonkeyWindowRules
    {
        public static readonly string[] IgnoreRules =
        {
            "[Ll]ibrary/",
            "[Tt]emp/",
            "[Oo]bj/",
            "[Bb]uild/",
            "[Bb]uilds/",
            "[Ll]ogs/",
            "[Uu]ser[Ss]ettings/"
        };

        public static readonly string[] LfsRules =
        {
            "*.psd filter=lfs diff=lfs merge=lfs -text",
            "*.psb filter=lfs diff=lfs merge=lfs -text",
            "*.fbx filter=lfs diff=lfs merge=lfs -text",
            "*.blend filter=lfs diff=lfs merge=lfs -text",
            "*.wav filter=lfs diff=lfs merge=lfs -text",
            "*.mp3 filter=lfs diff=lfs merge=lfs -text",
            "*.ogg filter=lfs diff=lfs merge=lfs -text",
            "*.mp4 filter=lfs diff=lfs merge=lfs -text",
            "*.mov filter=lfs diff=lfs merge=lfs -text",
            "*.unitypackage filter=lfs diff=lfs merge=lfs -text",
            "*.exr filter=lfs diff=lfs merge=lfs -text",
            "*.tga filter=lfs diff=lfs merge=lfs -text"
        };

        public static readonly string[] LfsPatterns =
        {
            "*.psd",
            "*.psb",
            "*.fbx",
            "*.blend",
            "*.wav",
            "*.mp3",
            "*.ogg",
            "*.mp4",
            "*.mov",
            "*.unitypackage",
            "*.exr",
            "*.tga"
        };
    }
}
