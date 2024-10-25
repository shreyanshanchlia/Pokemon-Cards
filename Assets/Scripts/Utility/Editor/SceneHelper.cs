using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility.Editor.Toolbar
{
    internal static class SceneHelper
    {
        private static string sceneToOpen;
        private static string prevScene;

        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying) EditorApplication.isPlaying = false;

            sceneToOpen = sceneName;
            prevScene = SceneManager.GetActiveScene().name;
            EditorApplication.update += OnUpdate;
        }

        public static void LoadPrevScene()
        {
            if (EditorApplication.isPlaying) EditorApplication.isPlaying = false;

            sceneToOpen = prevScene;
            prevScene = "";
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (sceneToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            EditorApplication.update -= OnUpdate;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // need to get scene via search because the path to the scene
                // file contains the package version so it'll change over time
                var guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    var scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);
                    if (prevScene != "") EditorApplication.isPlaying = true;
                }
            }

            sceneToOpen = null;
        }
    }
}