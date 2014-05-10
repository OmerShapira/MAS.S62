using UnityEngine;
using System.Collections;

public class Override : MonoBehaviour {

	FPSInputController input;
	CharacterMotor motor;
	MouseLook look;
	// Use this for initialization
	void Start () {
		input = GetComponent<FPSInputController>();
		motor = GetComponent<CharacterMotor>();
		look = GetComponent<MouseLook>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.LeftControl)){
			input.enabled = false;
			motor.enabled = false;
			look.enabled = false;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.LeftControl)){
			input.enabled = true;
			motor.enabled = true;
			look.enabled = true;
		}
	}
}
