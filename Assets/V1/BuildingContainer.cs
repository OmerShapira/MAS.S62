
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[ExecuteInEditMode]
public class BuildingContainer : MonoBehaviour {

	const float DISCRETE_EVENT_THRESHOLD = 1f;

//	public Vector3 floorStandardMeasuresMeters;
	public GameObject[] floorPrototypes;
	public int numFloors;
	public float floorHeight;
	private Stack<GameObject> floors;
	
	public float HeightConstraint;

	void Awake() {
		floors = new Stack<GameObject>();
	}

	void Start () {

	}
	

	void Update () {
	}

	public void Build(){
		foreach (var i in Enumerable.Range(0, numFloors - 1)){
			AddFloor();
		}
		SendMessage("UpdateColliders");
		SendMessageUpwards("UpdateColliders");
	}

	public void RemoveFloor(){
		if (floors.Count > 0) { 
			Destroy(floors.Pop()); 
		}
	}

	public void AddFloor(){
		floors.Push(AddRandomFloor(floors.Count));
	}

	public void UpdateFloorHeight(float height){
		floorHeight = height;
		foreach(var floor in floors){
			var p = floor.transform.position;

			p.y = GetHeightForFloor(floor.GetComponent<FloorContainer>().number);

			floor.transform.position = p;
		}
	}

	public void UpdateNumFloors(Vector2 input){
		if (Mathf.Abs(input.y) > DISCRETE_EVENT_THRESHOLD){
			if (input.y > 0){
				AddFloor();
			} else {
				RemoveFloor();
			}
			SendMessage("UpdateColliders");
			SendMessageUpwards("UpdateColliders");
		}
	}


	GameObject AddRandomFloor(int whichFloor){
		int skip = Mathf.FloorToInt(Random.value * numFloors);
		//TODO: Add check if this fits the first floor
		var newFloor = GameObject.Instantiate(
						floorPrototypes[skip % floorPrototypes.Length], 
						new Vector3(transform.position.x, GetHeightForFloor(whichFloor), transform.position.z),
						transform.rotation) as GameObject;

		newFloor.GetComponent<FloorContainer>().number = whichFloor + 1;
		//Adapt size to standard and fit constraints


//		var standardMeasures = floorStandardMeasuresMeters;
		var standardMeasures = FindObjectOfType<SceneManagerState>().floorStandardMeasuresMeters;

		var scale = newFloor.transform.localScale;

		scale.x *= transform.localScale.x / standardMeasures.x;
		scale.y *= floorHeight / standardMeasures.y;
		scale.z *= transform.localScale.z / standardMeasures.z;

		newFloor.transform.localScale = scale;

		newFloor.transform.parent = transform;
		return newFloor;
	}

	private float GetHeightForFloor(int whichFloor){
		return transform.position.y + whichFloor * floorHeight;
	}
	
}
