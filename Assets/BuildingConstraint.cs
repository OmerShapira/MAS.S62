using UnityEngine;
using System.Collections;

public class BuildingConstraint : MonoBehaviour {

	public Vector4 StreetMarginConstraintNormalized;
	public SceneManagerState manager;
	
	void Start () {
		manager = FindObjectOfType<SceneManagerState>();
		var sidewalk = GetComponentInChildren<Sidewalk>();
		sidewalk.transform.localScale = new Vector3(manager.buildingSizeMeters.x, manager.buildingSizeMeters.y);
		var building = GetComponentInChildren<BuildingContainer>();
		var scaleMax = new Vector3(manager.buildingSizeMeters.x, 1, manager.buildingSizeMeters.z);
		var scaleMinFactor = (new Vector3(1,1,1) -  new Vector3(StreetMarginConstraintNormalized.y + StreetMarginConstraintNormalized.w, 0, StreetMarginConstraintNormalized.x + StreetMarginConstraintNormalized.z));
		building.transform.localScale = new Vector3(scaleMax.x * scaleMinFactor.x, 1, scaleMax.z * scaleMinFactor.z);


		//TODO: Check if this needs to change with global coordinates to a local position one frame above (this one)
		building.transform.localPosition = new Vector3(StreetMarginConstraintNormalized.y - StreetMarginConstraintNormalized.w, 0, StreetMarginConstraintNormalized.x - StreetMarginConstraintNormalized.z);
		
		building.Build();
		GetComponent<ContextDisplay>().UpdateColliders();
	}

	void Update () {
		
	}

	public void SetStreetMarginConstraints(Vector4 constraints){
		StreetMarginConstraintNormalized = constraints;
		BroadcastMessage("SetStreetMarginConstraints", constraints);
	}
}
