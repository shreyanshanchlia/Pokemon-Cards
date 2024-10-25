using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility
{
    public class ComponentStarter : MonoBehaviour
    {
        [HideInInspector] public Component[] attachedComponents;

        public Vector2 startDelayRange;
        public string selectedComponentName;

        private void Awake()
        {
            Invoke(nameof(StartComponent), Random.Range(startDelayRange.x, startDelayRange.y));
        }

        private void StartComponent()
        {
            if (attachedComponents is {Length: > 0})
            {
                var selectedComponent = Array.Find(attachedComponents,
                    component => component.GetType().Name == selectedComponentName);

                if (selectedComponent == null)
                {
                    Debug.LogError($"Component {selectedComponentName} not found on {gameObject.name}");
                }
                else
                {
                    var component = selectedComponent as MonoBehaviour;
                    if (component != null)
                        component.enabled = true;
                }
            }
            else
            {
                Debug.LogError($"No components attached for {gameObject.name}");
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ComponentStarter))]
    public class ComponentStarterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var componentStarter = (ComponentStarter) target;

            EditorGUI.BeginChangeCheck();
            DrawPropertiesExcluding(serializedObject, "attachedComponents", "selectedComponentName");
            if (componentStarter.attachedComponents != null)
            {
                componentStarter.attachedComponents = componentStarter.GetComponents<Component>();
                var componentNames = new string[componentStarter.attachedComponents.Length];
                for (var i = 0; i < componentStarter.attachedComponents.Length; i++)
                    componentNames[i] = componentStarter.attachedComponents[i].GetType().Name;

                var selectedIndex = EditorGUILayout.Popup("Selected Component",
                    Array.IndexOf(componentNames, componentStarter.selectedComponentName), componentNames);

                if (selectedIndex >= 0 && selectedIndex < componentNames.Length)
                    if (componentStarter.selectedComponentName != componentNames[selectedIndex])
                    {
                        Undo.RecordObject(componentStarter, "Selected Component");
                        componentStarter.selectedComponentName = componentNames[selectedIndex];
                        Undo.FlushUndoRecordObjects();
                    }
            }

            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}