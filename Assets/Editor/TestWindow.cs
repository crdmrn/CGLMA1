using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestWindow : EditorWindow {

	//TestWindow window;

	[MenuItem("Test/TestWindow")]
	static void Init() {
		TestWindow window = (TestWindow)EditorWindow.GetWindow(typeof(TestWindow));
		window.Show();
	}

	void OnGUI() {
		if (GameManager.Instance == null) {
			EditorGUILayout.HelpBox("Start the game to see something", MessageType.Error);
			//EditorGUILayout.beg
		} else {
			foreach (GameManager.LevelScore c in GameManager.Instance.numbers) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(c.levelName);
				EditorGUILayout.LabelField(c.score.ToString());
				EditorGUILayout.EndHorizontal();
			}
		}

	}

}
