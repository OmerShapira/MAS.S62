using UnityEngine;
using System.Collections;

public class FloorContainer : MonoBehaviour {

	private int _number = -1;
	public int number{
		get { return _number; }
		set { _number = value;}
	}

	void Start (){
		gameObject.name = "Floor #"+number;
	}
	
}
