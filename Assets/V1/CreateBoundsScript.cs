using UnityEngine;
using System.Collections;

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class CreateBoundsScript : MonoBehaviour {

	public Vector3 standardSize;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Calculate(){
		HashSet<IndexedLine> lines = new HashSet<IndexedLine>();
		Mesh modelMesh = GetComponent<MeshFilter>().sharedMesh;
		Vector3 corner = Vector3.zero;
		Vector3 axisLine = Vector3.right;
		for (int i = 0; i < modelMesh.triangles.Length - 1; i++){
			var line = new IndexedLine(modelMesh.triangles[i], modelMesh.triangles[i+1]);
			var lineDirection  = modelMesh.vertices[line.a] - modelMesh.vertices[line.b];
			var tempCorner = modelMesh.vertices[line.b];
			// if it's a vertical, ignore it.
			if (Mathf.Abs(lineDirection.y) > Mathf.Epsilon){
				//check if it's a shared line
				if (lines.Contains(line)){
					//if so, save and quit
					axisLine = lineDirection;
					corner = tempCorner;
					break;
				}
			}
		}


		var newObj = new GameObject();
		gameObject.transform.position = transform.position;
		//rotate the mesh to conform to X and begin at 0,0,0
		var rotation = Quaternion.FromToRotation(axisLine, Vector3.right);
		Vector3[] newVerts = new Vector3[modelMesh.vertices.Length];
		for(int i = 0 ; i < modelMesh.vertexCount ; i ++){
			if (modelMesh.vertices[i] != corner){
				newVerts[i] =  rotation * (modelMesh.vertices[i] - corner);
			} else {
				newVerts[i] = modelMesh.vertices[i] - corner;
			}
		}
		Mesh newMesh = new Mesh();
		newMesh.vertices = newVerts;
		newMesh.triangles = modelMesh.triangles;
		newMesh.uv = modelMesh.uv;
		//calculate AABB
		newMesh.RecalculateBounds();
		newMesh.RecalculateNormals();
		standardSize = newMesh.bounds.size;
		//Create collider and transform
		newObj.AddComponent<MeshFilter>();
		newObj.GetComponent<MeshFilter>().sharedMesh = newMesh;
		newObj.AddComponent<MeshRenderer>();
		newObj.GetComponent<MeshRenderer>().sharedMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
		newObj.AddComponent<BoxCollider>();
		newObj.GetComponent<BoxCollider>().center = newMesh.bounds.center;
		newObj.GetComponent<BoxCollider>().size = newMesh.bounds.size;
		//TODO: Test correctness
		newObj.transform.position = gameObject.transform.position;
		//TODO: Test order
		newObj.transform.rotation = transform.rotation; 
		newObj.transform.Rotate(Quaternion.FromToRotation(Vector3.right, axisLine).eulerAngles);

		//Set new name
		newObj.name = gameObject.name + " - Standard";
		newObj.transform.parent = gameObject.transform.parent;
	
	}

	private class IndexedLine{
		public readonly int a, b;
		public IndexedLine(int a, int b){
			if (b > a){
				this.a = a;
				this.b = b;
			} else {
				this.b = a;
				this.a = b;
			}
		}
	}
}
