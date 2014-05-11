using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameUtils;

//[ExecuteInEditMode]
public class GenericContextDisplay : MonoBehaviour {
	
	public string[] DisplayNames;
	public string[] Messages;
	
	SceneManagerState manager;
	GameObject selected;
	Collider ctxCollider;
	public Material selectionMaterial;
	
	void Awake(){
		manager = FindObjectOfType<SceneManagerState>();
		ctxCollider = GetComponent<Collider>();
		UpdateColliders();
	}
	
	public void SetSelection(GameObject obj){
		Destroy(selected);
		if (obj.Equals(gameObject)){
			SetSelectionCube();
			manager.UpdateParamDisplay(DisplayNames);
		}
	}
	
	public void UpdateColliders(){
		BroadcastMessage("RecalculateBounds");
	}
	
	public void UpdateParam(ParamData paramData){
		SendMessage(Messages[paramData.number], paramData.data);
	}
	
	private void SetSelectionCube(){
		if (gameObject.CompareTag("SceneManager")) {
			return;
		}
		selected = new GameObject();
		selected.name = "Selection";
		selected.transform.parent = transform;
		selected.transform.localPosition = Vector3.zero;
		selected.transform.localScale = Vector3.one * 1.05f;
		selected.AddComponent<MeshFilter>();
		selected.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().sharedMesh;
		selected.AddComponent<MeshRenderer>();
		selected.GetComponent<MeshRenderer>().material = selectionMaterial;


//		selected = (GameObject) GameObject.Instantiate(selectionCube, transform.localToWorldMatrix.MultiplyPoint3x4(collider.center), gameObject.transform.rotation);
	}
	
}
