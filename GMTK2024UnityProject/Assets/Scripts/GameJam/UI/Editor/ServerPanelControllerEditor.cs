using UnityEditor;

using UnityEngine;

namespace GameJam.Editor {
  [CustomEditor(typeof(ServerPanelController))]
  public sealed class ServerPanelControllerEditor : UnityEditor.Editor {
    ServerPanelController _controller;

    private void OnEnable() {
      _controller = (ServerPanelController) target;
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      EditorGUILayout.Separator();

      EditorGUILayout.LabelField(nameof(ServerPanelController), EditorStyles.boldLabel);
      EditorGUILayout.Separator();

      DrawPanelControls();
      EditorGUILayout.Separator();

      using (new EditorGUI.DisabledScope(!Application.isPlaying)) {
        DrawUserControls();
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

    string _userValue;
    string _powerValue;
    float _heatValue;

    private void DrawUserControls() {
      GUILayout.BeginVertical("ValueControls", GUI.skin.window);
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetUserValue", GUILayout.Width(110f))) {
        _controller.SetUserValue(_userValue);
      }

      _userValue = EditorGUILayout.TextField(_userValue);

      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetPowerValue", GUILayout.Width(110f))) {
        _controller.SetPowerValue(_powerValue);
      }

      _powerValue = EditorGUILayout.TextField(_powerValue);

      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetHeatValue", GUILayout.Width(110f))) {
        _controller.SetHeatValue(_heatValue);
      }

      _heatValue = EditorGUILayout.FloatField(_heatValue);

      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }
  }
}
