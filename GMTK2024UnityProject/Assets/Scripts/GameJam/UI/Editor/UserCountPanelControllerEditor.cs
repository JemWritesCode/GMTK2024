using UnityEditor;

using UnityEngine;

namespace GameJam.Editor {
  [CustomEditor(typeof(UserCountPanelController))]
  public sealed class UserCountPanelControllerEditor : UnityEditor.Editor {
    UserCountPanelController _controller;

    private void OnEnable() {
      _controller = (UserCountPanelController) target;
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      EditorGUILayout.Separator();

      EditorGUILayout.LabelField(nameof(UserCountPanelControllerEditor), EditorStyles.boldLabel);
      EditorGUILayout.Separator();

      DrawPanelControls();
      EditorGUILayout.Separator();

      using (new EditorGUI.DisabledScope(!Application.isPlaying)) {
        DrawUserCountControls();
      }
    }

    private void DrawPanelControls() {
      GUILayout.BeginHorizontal("PanelControls", GUI.skin.window);

      using (new EditorGUI.DisabledScope(!Application.isPlaying)) {
        if (GUILayout.Button("ShowPanel")) {
          _controller.ShowPanel();
        }

        if (GUILayout.Button("HidePanel")) {
          _controller.HidePanel();
        }
      }

      GUILayout.EndHorizontal();
    }

    int _userCountValue;

    private void DrawUserCountControls() {
      GUILayout.BeginVertical("UserCountControls", GUI.skin.window);
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetUserCountValue", GUILayout.Width(125f))) {
        _controller.SetUserCountValue(_userCountValue);
      }

      _userCountValue = EditorGUILayout.IntField(_userCountValue);

      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }
  }
}
