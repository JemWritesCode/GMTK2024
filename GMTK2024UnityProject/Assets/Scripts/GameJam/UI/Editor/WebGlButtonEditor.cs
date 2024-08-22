using UnityEditor;
using UnityEditor.UI;

namespace GameJam.Editor {

  [CustomEditor(typeof(WebGlButton))]
  public sealed class WebGlButtonEditor : ButtonEditor {
    SerializedProperty _onPointerDownClick;

    protected override void OnEnable() {
      base.OnEnable();

      _onPointerDownClick = serializedObject.FindProperty("<OnPointerDownClick>k__BackingField");
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      EditorGUILayout.Space();
      serializedObject.Update();
      EditorGUILayout.PropertyField(_onPointerDownClick);
      serializedObject.ApplyModifiedProperties();
    }
  }
}
