using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

namespace Utility.Templates
{
    [ExecuteAlways]
    public class SingleScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                    FindInstance();
                return _instance;
            }
        }

        [ExecuteAlways]
        private void Awake()
        {
            _instance = instance;
        }
        private static void FindInstance()
        {
#if UNITY_EDITOR
            var allSources = AssetDatabase.FindAssets($"t:{typeof(T)}");
            if (allSources == null || allSources.Length == 0) _instance = null;
            else _instance = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(allSources[0]));
#else
            var allSources = Resources.LoadAll<T>("");
            if (allSources == null || allSources.Length == 0) _instance = null;
            else _instance = allSources[0];
#endif       
            if (_instance == null) Debug.LogError($"{typeof(T)} not found in Assets. Create one!");
            else if (allSources!.Length > 1) Debug.LogError($"Multiple {typeof(T)} found in Assets. Remove one!");
        }

#if UNITY_EDITOR
        public static T CreateInstance()
        {
            var scriptableObject = CreateInstance<T>();
            var path = "Assets/Data/" + typeof(T) + ".asset";
            AssetDatabase.CreateAsset(scriptableObject, path);
            _instance = scriptableObject;
            return scriptableObject;
        }
#endif
    }
    
#if UNITY_EDITOR
    public class SingleScriptableEditorWindow<T> : OdinEditorWindow where T : SingleScriptableObject<T>
    {
        [UsedImplicitly] [SerializeField] [InlineEditor(InlineEditorModes.FullEditor, Expanded = true)]
        private T data;

        protected override void OnEnable()
        {
            data = SingleScriptableObject<T>.instance;
        }

        [ShowIf("@data == null")]
        [Button("Create Data")]
        private void CreateData()
        {
            data = SingleScriptableObject<T>.CreateInstance();
        }
    }
#endif
}
