using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	public GameObject ChangeSkin;
	public GameObject Pointer;
	public GameObject PointIn;
	public GameObject PointOut;
	public GameObject GoIn;
	public GameObject GoOut;
	

	// Use this for initialization
	void Start () {
		ActivateCursor(Pointer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ActivateCursor(GameObject cursor){
		GameObject activePointer = GameObject.Instantiate(cursor) as GameObject;
		activePointer.transform.parent = transform;
		activePointer.transform.localPosition = Vector3.zero;
		activePointer.transform.localRotation = Quaternion.Euler(270,0,0);
	}
		
	public void ActivateCursor(string cursorName){
		switch (cursorName){
		case "ChangeSkin": ActivateCursor(ChangeSkin); break;
		case "PointIn": ActivateCursor(PointIn); break;
		case "PointOut": ActivateCursor(PointOut); break;
		case "GoIn": ActivateCursor(GoIn); break;
		case "GoOut": ActivateCursor(GoOut); break;
		default: ActivateCursor(Pointer); break;
		}
	}

}
