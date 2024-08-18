using UnityEditor;

using UnityEngine;

namespace GameJam.Editor {
  [CustomEditor(typeof(StartScreenUIController))]
  public sealed class StartScreenUIControllerEditor : UnityEditor.Editor {
    StartScreenUIController _controller;

    private void OnEnable() {
      _controller = (StartScreenUIController) target;
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      EditorGUILayout.Separator();

      EditorGUILayout.LabelField(nameof(StartScreenUIControllerEditor), EditorStyles.boldLabel);
      EditorGUILayout.Separator();

      using (new EditorGUI.DisabledScope(!Application.isPlaying)) {
        if (GUILayout.Button("AnimateIntro")) {
          _controller.AnimateIntro();
        }
      }
    }
  }
}
