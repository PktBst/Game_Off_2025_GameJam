using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class ButtonEditor : Editor
{
    private Dictionary<string, object[]> _paramValues = new Dictionary<string, object[]>();
    private Dictionary<string, bool> _foldouts = new Dictionary<string, bool>();

    public override void OnInspectorGUI()
    {
        // Draw the normal inspector
        DrawDefaultInspector();

        // Get the target object
        var targetType = target.GetType();

        // Find methods with [Button]
        // We use 'GetMethods' to find functions hidden in the class
        var methods = targetType.GetMethods(
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0)
            .ToArray();

        if (methods.Length == 0) return;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        foreach (var method in methods)
        {
            DrawMethod(method);
        }
    }

    void DrawMethod(MethodInfo method)
    {
        var attr = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
        string label = string.IsNullOrEmpty(attr.Label) ? ObjectNames.NicifyVariableName(method.Name) : attr.Label;
        ParameterInfo[] parameters = method.GetParameters();

        // If parameters exist, wrap in a visual box
        if (parameters.Length > 0)
        {
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUI.indentLevel++;
        }

        // --- Parameter Management ---
        string methodKey = target.GetInstanceID() + "_" + method.Name;

        // Initialize params if they don't exist yet
        if (!_paramValues.ContainsKey(methodKey) || _paramValues[methodKey].Length != parameters.Length)
        {
            object[] defaults = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Type pt = parameters[i].ParameterType;
                if (pt == typeof(string)) defaults[i] = "";
                else if (pt.IsValueType && !pt.IsEnum) defaults[i] = Activator.CreateInstance(pt);
                else defaults[i] = null;
            }
            _paramValues[methodKey] = defaults;
        }

        object[] currentParams = _paramValues[methodKey];

        // Draw Fields
        for (int i = 0; i < parameters.Length; i++)
        {
            string paramId = methodKey + "_p" + i;
            currentParams[i] = DrawAnyField(parameters[i].ParameterType, parameters[i].Name, currentParams[i], paramId);
        }

        // --- The Button ---
        if (GUILayout.Button(label, GUILayout.Height(parameters.Length > 0 ? 24 : 30)))
        {
            foreach (var t in targets)
            {
                Undo.RecordObject(t, label); // Allow Ctrl+Z
                method.Invoke(t, currentParams);
            }
        }

        if (parameters.Length > 0)
        {
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        else
        {
            // Add a tiny space for button-only functions so they don't touch
            GUILayout.Space(2);
        }
    }

    // Recursive Field Drawer
    object DrawAnyField(Type type, string name, object value, string keyId)
    {
        string label = ObjectNames.NicifyVariableName(name);

        if (type == typeof(int)) return EditorGUILayout.IntField(label, (int)value);
        if (type == typeof(float)) return EditorGUILayout.FloatField(label, (float)value);
        if (type == typeof(string)) return EditorGUILayout.TextField(label, (string)value ?? "");
        if (type == typeof(bool)) return EditorGUILayout.Toggle(label, (bool)value);
        if (type == typeof(Vector3)) return EditorGUILayout.Vector3Field(label, (Vector3)value);
        if (type == typeof(Vector2)) return EditorGUILayout.Vector2Field(label, (Vector2)value);
        if (type == typeof(Color)) return EditorGUILayout.ColorField(label, (Color)value);
        if (type.IsEnum) return EditorGUILayout.EnumPopup(label, (Enum)value);
        if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type, true);

        // Custom Class Drawer (Recursive)
        if (!type.IsPrimitive && !type.IsArray && type != typeof(string))
        {
            if (value == null && HasParameterlessConstructor(type))
                value = Activator.CreateInstance(type);

            if (value != null)
            {
                if (!_foldouts.ContainsKey(keyId)) _foldouts[keyId] = true;
                _foldouts[keyId] = EditorGUILayout.Foldout(_foldouts[keyId], label, true);

                if (_foldouts[keyId])
                {
                    EditorGUI.indentLevel++;
                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        field.SetValue(value, DrawAnyField(field.FieldType, field.Name, field.GetValue(value), keyId + "_" + field.Name));
                    }
                    EditorGUI.indentLevel--;
                }
            }
            return value;
        }
        return value;
    }

    bool HasParameterlessConstructor(Type t) => t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
}