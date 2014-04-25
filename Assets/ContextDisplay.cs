using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameUtils;

public class ContextDisplay : MonoBehaviour {

	public string[] DisplayNames;
	public string[] Messages;

	SceneManagerState manager;
	GameObject selected;
	BoxCollider boxCollider;
	public GameObject selectionCube;

	void Awake(){
		manager = FindObjectOfType<SceneManagerState>();
		boxCollider = GetComponent<BoxCollider>();
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
		var colliders = new List<Collider>();
		foreach (Transform child in transform){
			//must do this manually, and not via message in order to ensure execution.
			var contextDisplay = child.gameObject.GetComponent<ContextDisplay>();
			if (contextDisplay){
				contextDisplay.UpdateColliders();
			}
			var childCollider = child.gameObject.GetComponent<Collider>();
			if (childCollider){ 
				colliders.Add(childCollider); 
			}
		}

		if (colliders.Count > 0){
			var localBounds = new Bounds();
			foreach (var col in colliders){
				localBounds.Encapsulate(new Bounds(transform.worldToLocalMatrix.MultiplyPoint(col.bounds.center), transform.worldToLocalMatrix.MultiplyVector(col.bounds.size)));
			}
			boxCollider.size = localBounds.size;
			boxCollider.center = localBounds.center;
		}
	}

	public void UpdateParam(ParamData paramData){
		SendMessage(Messages[paramData.number], paramData.data);
	}

	private void SetSelectionCube(){
		selected = (GameObject) GameObject.Instantiate(selectionCube, transform.localToWorldMatrix.MultiplyPoint3x4(boxCollider.center), gameObject.transform.rotation);
		selected.transform.localScale = transform.localToWorldMatrix.MultiplyVector(boxCollider.size) * 1.05f;
		selected.transform.parent = transform;
	}

}
