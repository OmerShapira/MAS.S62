﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Building : MonoBehaviour {

	public GameObject prototype;
	public Material[] skins;
	
	public void InitializeStructure(){
		if(transform.childCount > 0){
			var unsorted = new List<Floor>();
			foreach (Transform f in transform){
				var tempf = f.gameObject.GetComponent<Floor>();
				if (tempf) {DestroyImmediate(tempf);} //FIXME: Probably wrong
				f.gameObject.AddComponent<Floor>();
				unsorted.Add(f.gameObject.GetComponent<Floor>());
			}
			var sorted = (from f in unsorted
              let y = f.transform.position.y + f.GetComponent<MeshFilter>().sharedMesh.vertices.Select(x => (f.transform.localToWorldMatrix.MultiplyPoint3x4(x)).y).Average()
				orderby y
	         	select f).ToList();
			for (int i = 0 ; i < sorted.Count ; i ++){
				sorted[i].floorNumber = i;
			}
			//make some prototypes
			if ((prototype == null)){
				prototype = sorted[0].gameObject;
			}
			if (skins == null || skins.Length == 0){

			}
		}
	}


//	public void RebuildFromList(){
//
//	}

	public void Build(){

	}
}
