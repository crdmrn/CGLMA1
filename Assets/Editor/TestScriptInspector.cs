using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameManager))]
public class TestScriptInspector : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		GameManager s = (GameManager)target;
		serializedObject.Update();

		// start checking if properties are changed
		EditorGUI.BeginChangeCheck();

		// draw properties
		GameManager.LevelScore lineToRemove = null;
		foreach (GameManager.LevelScore c in s.numbers) {
			EditorGUILayout.BeginHorizontal();
			c.levelName = EditorGUILayout.TextField(c.levelName);
			c.score = EditorGUILayout.FloatField(c.score);
			if (GUILayout.Button("X")) {
				lineToRemove = c;
			}
			EditorGUILayout.EndHorizontal();
		}
		if (lineToRemove != null)
			s.numbers.Remove(lineToRemove);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Press")) {
			s.numbers.Add(new GameManager.LevelScore());
		}
		EditorGUILayout.EndHorizontal();

		// if things were changed, apply the changes 
		if (EditorGUI.EndChangeCheck()) {
			serializedObject.ApplyModifiedProperties();
			if (!Application.isPlaying)
				EditorSceneManager.MarkAllScenesDirty();
		}
	}

}
