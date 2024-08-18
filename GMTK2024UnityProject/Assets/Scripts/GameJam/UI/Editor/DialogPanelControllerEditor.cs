using UnityEditor;

using UnityEngine;

namespace GameJam.Editor {
  [CustomEditor(typeof(DialogPanelController))]
  public sealed class DialogPanelControllerEditor : UnityEditor.Editor {
    DialogPanelController _controller;

    private void OnEnable() {
      _controller = (DialogPanelController) target;
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      EditorGUILayout.Separator();

      EditorGUILayout.LabelField(nameof(DialogPanelControllerEditor), EditorStyles.boldLabel);
      EditorGUILayout.Separator();

      DrawPanelControls();
      EditorGUILayout.Separator();
    }

    private void DrawPanelControls() {
      GUILayout.BeginHorizontal("Panel Controls", GUI.skin.window);

      using (new EditorGUI.DisabledScope(!Application.isPlaying)) {
        if (GUILayout.Button("Show Panel")) {
          _controller.ShowPanel();
        }

        if (GUILayout.Button("Hide Panel")) {
          _controller.HidePanel();
        }
      }

      GUILayout.EndHorizontal();
    }
  }
}
