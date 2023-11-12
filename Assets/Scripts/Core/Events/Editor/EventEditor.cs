using UnityEditor;
using UnityEngine;

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