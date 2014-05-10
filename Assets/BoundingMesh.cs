using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]

public class BoundingMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RecalculateBounds(){
		Transform[] transforms =(Transform[]) GetComponentsInChildren<Transform>();
		GetComponent<MeshFilter>().sharedMesh = ConvexHullAlgorithms.GetBounds(transforms.Select(x => x.gameObject), transform.worldToLocalMatrix);
		GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
	}
}
