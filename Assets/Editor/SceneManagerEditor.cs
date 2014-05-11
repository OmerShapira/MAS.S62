using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SceneManagerState))]
public class SceneManagerEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		SceneManagerState manager = (SceneManagerState)target;
		if(GUILayout.Button("Save To XML"))
		{
			manager.NewXMLSceneFromState(manager.fileName);
		}

		if(GUILayout.Button("Rebuild Scene"))
		{
//			manager.BroadcastMessage("InitializeStructure");
			manager.InitializeStructure();
		}
	}
}
