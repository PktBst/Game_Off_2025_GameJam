using System;
using System.Reflection;
using UnityEngine;

public static class ComponentUtility
{
    public static T AddComponentWithValues<T>(GameObject destination, T source)
        where T : Component
    {
        // Add the EXACT runtime type (Sirens, Cavemen, etc.)
        Type type = source.GetType();
        T copy = destination.AddComponent(type) as T;

        // Copy only safe fields
        var fields = type.GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic
        );

        foreach (var field in fields)
        {
            // Skip events / delegates
            if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                continue;

            // Copy value
            field.SetValue(copy, field.GetValue(source));
        }

        return copy;
    }
}
