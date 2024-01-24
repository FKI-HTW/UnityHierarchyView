using UnityEditor;
using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
	[CustomEditor(typeof(HierarchyManager))]
	public class HierarchyManagerEditor : Editor
	{
		private Editor _settingsEditor = null;
		public Editor SettingsEditor
		{
			get
			{
				if (_settingsEditor == null)
					_settingsEditor = CreateEditor(((HierarchyManager)target).HierarchyConfiguration);
				return _settingsEditor;
			}
		}

		private SerializedProperty _hierarchyConfig;
		private SerializedProperty _hierarchyContainer;

		public void OnEnable()
		{
			_hierarchyConfig = serializedObject.FindProperty("_hierarchyConfiguration"); ;
			_hierarchyContainer = serializedObject.FindProperty("_hierarchyContainer"); ;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(_hierarchyConfig, new GUIContent("Hierarchy Configuration"));
			if (_hierarchyConfig.objectReferenceValue != null)
			{
				EditorGUI.indentLevel++;
				SettingsEditor.OnInspectorGUI();
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField(_hierarchyContainer);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
