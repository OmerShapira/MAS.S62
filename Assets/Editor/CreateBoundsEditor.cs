using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CreateBoundsScript))]
public class CreateBoundsEditor : Editor{
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		CreateBoundsScript createBounds = (CreateBoundsScript)target;
		if(GUILayout.Button("Calculate"))
		{
			createBounds.Calculate();
		}
	}
}
