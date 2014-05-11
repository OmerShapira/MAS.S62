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

//	void OnEnable(){
//		data = new FloorData();
//		data.ID = GetInstanceID();
//	}

	public void SetData(FloorData data){
		if (data.ID == GetInstanceID()){
			this._data = data;
		} else {
			throw new UnityException("ID Mismatch");
		}
	}

	public void SetType(FloorType type){
		this._data.type = type;
		//TODO: Set dirty
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