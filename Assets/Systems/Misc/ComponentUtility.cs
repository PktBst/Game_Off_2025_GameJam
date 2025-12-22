using UnityEngine;
using System.Reflection;

public static class ComponentUtility
{
    public static T AddComponentWithValues<T>(GameObject destination, T source) where T : Component
    {
        // 1. Add the component type
        T copy = destination.AddComponent(source.GetType()) as T;
        var type = source.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            field.SetValue(copy, field.GetValue(source));
        }

        // 2. Copy the fields (this happens BEFORE Start)

        return copy;
    }
}