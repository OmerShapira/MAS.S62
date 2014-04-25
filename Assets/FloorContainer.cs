using UnityEngine;
using System.Collections;

public class FloorContainer : MonoBehaviour {

	[Range(1,100)]
	public float colorSensitivity;
	Material mat;
	private int _number = -1;
	public int number{
		get { return _number; }
		set { _number = value;}
	}

	void Awake(){
		mat = GetComponentInChildren<Renderer>().material;
	}

	void Start (){
		gameObject.name = "Floor #"+number;
	}

	public void UpdateR (Vector2 data){
		var c = mat.color;
		c.r = Mathf.Clamp01(c.r + data.x * colorSensitivity / Screen.width);
		mat.color = c;
	}

	public void UpdateG (Vector2 data){
		var c = mat.color;
		c.g = Mathf.Clamp01(c.g + data.x * colorSensitivity / Screen.width);
		mat.color = c;
	}

	public void UpdateB (Vector2 data){
		var c = mat.color;
		c.b = Mathf.Clamp01(c.b + data.x * colorSensitivity / Screen.width);
		mat.color = c;
	}

}
