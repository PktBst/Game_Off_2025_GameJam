#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using UnityEditor;
[InitializeOnLoad]
public class SceneToolbarRight
{
    static SceneToolbarRight()
    {
        EditorApplication.update += OnUpdate;
    }

    static void OnUpdate()
    {
        // 1. Find the Unity Toolbar
        var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);

        if (toolbars.Length == 0) return;

        // 2. Get the root VisualElement via reflection
        var root = toolbarType.GetField("m_Root", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(toolbars[0]) as VisualElement;

        // 3. Find the Right-Aligned Zone
        // "ToolbarZoneRightAlign" holds the Layout, Layers, and Account buttons.
        var zone = root?.Q("ToolbarZoneRightAlign");

        // Ensure we haven't already added our dropdown
        if (zone != null && zone.Q("SceneDropdownRight") == null)
        {
            var container = new IMGUIContainer(DrawSceneDropdown);
            container.name = "SceneDropdownRight";

            // 4. Insert at index 0 to put it to the LEFT of the default right-side buttons (Layers/Layout)
            // Or use zone.Add(container) to try putting it on the far right (Unity's own buttons might push it back).
            zone.Insert(6, container);

            // Stop the update loop once initialized
            EditorApplication.update -= OnUpdate;
        }
    }

    static void DrawSceneDropdown()
    {
        var activeScene = EditorSceneManager.GetActiveScene();
        string title = string.IsNullOrEmpty(activeScene.path) ? "Unsaved" : activeScene.name;

        // Draw the button
        // "minWidth" ensures it doesn't get crushed by other toolbar elements
        GUILayout.BeginHorizontal();
        if (EditorGUILayout.DropdownButton(new GUIContent(title, "Click to switch scenes"), FocusType.Passive, EditorStyles.toolbarDropDown, GUILayout.Width(130)))
        {
            GenericMenu menu = new GenericMenu();

            // 5. Scan ENTIRE project for scenes
            // "t:Scene" is the filter for .unity files
            string[] guids = AssetDatabase.FindAssets("t:Scene");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string name = Path.GetFileNameWithoutExtension(path);

                // Optional: Filter out packages to keep list clean
                if (path.StartsWith("Packages/")) continue;

                menu.AddItem(new GUIContent(name), activeScene.path == path, () => {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(path);
                });
            }

            menu.ShowAsContext();
        }
        GUILayout.EndHorizontal();
    }
}
#endif