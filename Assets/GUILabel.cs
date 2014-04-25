using UnityEngine;
using System.Collections;

public class GUILabel : MonoBehaviour {

	SceneManagerState manager;
//	GUIText guiText;
	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<SceneManagerState>();
//		guiText = GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = manager.context.name;
		transform.position = Camera.main.WorldToViewportPoint(manager.context.collider.bounds.center + manager.context.collider.bounds.extents);
	}
}
