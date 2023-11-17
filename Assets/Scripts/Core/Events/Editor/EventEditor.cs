#if UNITY_EDITOR
using Assets.Scripts.Core.Events;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        var e = target as GameEvent;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}
#endif