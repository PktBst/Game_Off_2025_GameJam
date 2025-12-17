using System;
using System.Diagnostics;
using UnityEngine;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : PropertyAttribute
{
    public string Label;
    public ButtonAttribute(string label = null)
    {
        this.Label = label;
    }
}