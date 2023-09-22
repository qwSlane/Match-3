using UnityEditor;

public class BuildScript
{
    [MenuItem("Build/Build Project")]
       public static void Build()
       {
           BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Build/MyGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
       }
}
