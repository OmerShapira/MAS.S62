using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;


[ExecuteInEditMode]
[RequireComponent(typeof(GenericContextDisplay))]
public class Floor : MonoBehaviour {

	FloorData _data;

	public FloorData data{
		get { return _data; } 
		set { SetData(value); }
	}

	public int floorNumber= -1;

	void Awake(){
		if (_data == null){
			_data = new FloorData();
			_data.ID = GetInstanceID();
		}
	}

	public void Build (FloorData data, int floorNumber){
		this.floorNumber = floorNumber;
		SetData(data);
		UpdateProperties();
	}

	public void SetData(FloorData data){
		this._data = data;
		this._data.ID = GetInstanceID();
	}

	public void SetType(int numtype){
		SetType ((FloorType) numtype);
	}

	public void SetType(FloorType type){
		this._data.type = type;
		//TODO: Set dirty
	}

	private void UpdateProperties(){

	}
}

[XmlType("Floor")]
public class FloorData{
	[XmlElement("ID")]
	public int ID = -1;
	[XmlEnum("FloorType")]
	public FloorType type;
}

[XmlType("FloorType")]
public enum FloorType{
	[XmlEnum(Name = "0")]
	Residential = 0, 
	[XmlEnum(Name = "1")]
	Commercial = 1
}