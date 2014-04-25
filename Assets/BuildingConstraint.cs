using UnityEngine;
using System.Collections;

public class BuildingConstraint : MonoBehaviour {

	public Vector4 StreetMarginConstraintNormalized;
	public SceneManagerState manager;
	const float INCREMENT_SIZE = 2f;
	Sidewalk sidewalk;
	BuildingContainer building;

	void Awake(){
		building = GetComponentInChildren<BuildingContainer>();
		manager = FindObjectOfType<SceneManagerState>();
		sidewalk = GetComponentInChildren<Sidewalk>();
	}

	void Start () {
		sidewalk.transform.localScale = new Vector3(manager.buildingSizeMeters.x, manager.buildingSizeMeters.y);
		SetBuildingSize(building);
		building.Build();
		GetComponent<ContextDisplay>().UpdateColliders();
	}

	void Update () {
		
	}

	public void UpdateSidewalkNE(Vector2 input){
		StreetMarginConstraintNormalized.x = Mathf.Clamp(StreetMarginConstraintNormalized.x + INCREMENT_SIZE * input.y / Screen.height, 0, 0.5f);
		StreetMarginConstraintNormalized.y = Mathf.Clamp(StreetMarginConstraintNormalized.y + INCREMENT_SIZE * input.x / Screen.width, 0, 0.5f);
		SetBuildingSize(building);
		SendMessage("UpdateColliders");
	}

	public void UpdateSidewalkSW(Vector2 input){
		StreetMarginConstraintNormalized.z = Mathf.Clamp(StreetMarginConstraintNormalized.z + INCREMENT_SIZE * input.y / Screen.height, 0, 0.5f);
		StreetMarginConstraintNormalized.w = Mathf.Clamp(StreetMarginConstraintNormalized.w + INCREMENT_SIZE * input.x / Screen.width, 0, 0.5f);
		SetBuildingSize(building);
		SendMessage("UpdateColliders");
	}

	public void SetStreetMarginConstraints(Vector4 constraints){
		StreetMarginConstraintNormalized = constraints;
		BroadcastMessage("SetStreetMarginConstraints", constraints);
	}

	private void SetBuildingSize(BuildingContainer building){
		var scaleMax = new Vector3(manager.buildingSizeMeters.x, 1, manager.buildingSizeMeters.z);
		var scaleMinFactor = (new Vector3(1,1,1) -  new Vector3(StreetMarginConstraintNormalized.y + StreetMarginConstraintNormalized.w, 0, StreetMarginConstraintNormalized.x + StreetMarginConstraintNormalized.z));
		building.transform.localScale = new Vector3(scaleMax.x * scaleMinFactor.x, 1, scaleMax.z * scaleMinFactor.z);
		
		//TODO: Check if this needs to change with global coordinates to a local position one frame above (this one)
		building.transform.localPosition = new Vector3(StreetMarginConstraintNormalized.y - StreetMarginConstraintNormalized.w, 0, StreetMarginConstraintNormalized.x - StreetMarginConstraintNormalized.z);
	}
}
