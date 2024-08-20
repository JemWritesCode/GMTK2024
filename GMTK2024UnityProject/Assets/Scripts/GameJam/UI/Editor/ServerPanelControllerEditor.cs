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

      EditorGUILayout.LabelField(nameof(ServerPanelControllerEditor), EditorStyles.boldLabel);
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

    int _userValue;
    int _powerValue;
    float _heatValue;
    bool _poweredOnValue;
    bool _fireOnValue;
    bool _virusOnValue;

    private void DrawUserControls() {
      GUILayout.BeginVertical("ValueControls", GUI.skin.window);
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetUserValue", GUILayout.Width(110f))) {
        _controller.SetUserValue(_userValue);
      }

      _userValue = EditorGUILayout.IntField(_userValue);

      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetPowerValue", GUILayout.Width(110f))) {
        _controller.SetPowerValue(_powerValue);
      }

      _powerValue = EditorGUILayout.IntField(_powerValue);

      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("SetHeatValue", GUILayout.Width(110f))) {
        _controller.SetHeatValue(_heatValue);
      }

      _heatValue = EditorGUILayout.FloatField(_heatValue);

      GUILayout.EndHorizontal();
      EditorGUILayout.Separator();
      GUILayout.BeginHorizontal();

      if (GUILayout.Button("PoweredOn")) {
        _poweredOnValue = !_poweredOnValue;
        _controller.SetPoweredOnValue(_poweredOnValue);
      }

      if (GUILayout.Button("FireOn")) {
        _fireOnValue = !_fireOnValue;
        _controller.SetFireOnValue(_fireOnValue);
      }

      if (GUILayout.Button("VirusOn")) {
        _virusOnValue = !_virusOnValue;
        _controller.SetVirusOnValue(_virusOnValue);
      }

      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }
  }
}
