using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(GenericContextDisplay))]
public class Compound : MonoBehaviour {

	CompoundData _data;

	public CompoundData data {
		get {return _data;}
		set {SetData(value);}
	}

	void Awake(){
		_data = new CompoundData();
		_data.ID = GetInstanceID();
		PrepareDataForSerialization();
	}

	public void PrepareDataForSerialization(){
		_data.floors = GetComponentsInChildren<Floor>().OrderBy(x => x.floorNumber).Select(x => x.data).ToList();
	}

	public void SetData(CompoundData data){
		if (data.ID ==  GetInstanceID()){
			//TODO: Solve this issue ; Don't really know where the building should go
		} else {
			throw new UnityException("ID Mismatch");
		}
	}

	public void InitializeStructure(){
		GetComponentInChildren<Building>().InitializeStructure();
		_data = new CompoundData();
		_data.ID = GetInstanceID();
		PrepareDataForSerialization();
	}

}

[XmlType("Compound")]
public class CompoundData{
	[XmlElement("ID")]
	public int ID = -1;
	[XmlElement("Sidewalk")]
	public Vector4 sidewalk = Vector4.zero;
	
	[XmlArray("Floors"), XmlArrayItem("Floor", typeof(FloorData))]
	public List<FloorData> floors;

	public void Add(FloorData data){
		floors.Add(data);
	}


}
