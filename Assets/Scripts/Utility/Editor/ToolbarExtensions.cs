using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;
using Debug = UnityEngine.Debug;

namespace Utility.Editor.Toolbar
{
    [InitializeOnLoad]
    public class ToolbarExtensions
    {
        static ToolbarExtensions()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
        }

        private static void OnLeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent(" Data", EditorGUIUtility.IconContent("d_CustomTool").image,
                    "Ping the game database")))
            {
                var path = "Assets/Resources/Data/";
                PingPath(path);
            }

            /*
            if (GUILayout.Button(new GUIContent("Play Scene", EditorGUIUtility.IconContent("PlayButton").image, "Play Login Scene")))
            {
                if (!EditorApplication.isPlaying)
                    SceneHelper.StartScene("SceneName");
                else
                    SceneHelper.LoadPrevScene();
            }
            */
        }

        private static void OnRightToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent(" Save Files", EditorGUIUtility.IconContent("Clipboard").image,
                    "Load the game save files folder"))) OpenSaveFilesFolder();
            if (GUILayout.Button(new GUIContent(" Scenes", "Ping the game scenes")))
            {
                var path = "Assets/Scenes/";
                PingPath(path);
            }
            
            /*
             if (GUILayout.Button(new GUIContent("Gameplay", EditorGUIUtility.IconContent("Mirror").image, "Open the gameplay scene")))
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene("Assets/Scenes/MainMenu/Gameplay.unity");
            */
            #region load scenes
            GUIContent scenesDropdownButtonLabel = new GUIContent("Scenes  \u002B");
            GUIContent[] environments = { new GUIContent("Scenes") };
            if (GUILayout.Button(scenesDropdownButtonLabel))
            {
                GenericMenu menu = new GenericMenu();
                foreach (var buildScene in EditorBuildSettings.scenes)
                {
                    string scenePath = buildScene.path;
                    string sceneName = Path.GetFileName(scenePath);
                    menu.AddItem(new GUIContent(sceneName), false, () =>
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            EditorSceneManager.OpenScene(scenePath);
                    });
                }
                menu.ShowAsContext();
            }
            #endregion
        }

        private static void PingPath(string path)
        {
            UnityEditor.EditorUtility.FocusProjectWindow();

            if (path[^1] == '/')
                path = path.Substring(0, path.Length - 1);

            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);

            //Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        private static void OpenSaveFilesFolder()
        {
            var folderPath = Application.persistentDataPath.Replace("/", "\\");
            if (Directory.Exists(folderPath))
            {
                var startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                Debug.Log($"{folderPath} Directory does not exist!");
            }
        }
    }
}