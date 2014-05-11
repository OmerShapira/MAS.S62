using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(GenericContextDisplay))]
public class Block : MonoBehaviour {

	BlockData _data;

	public BlockData data {
		get {return _data;}
		set {SetData(value);}
	}

	void Awake(){
		_data = new BlockData();
		_data.ID = GetInstanceID();
	}

	void Setup(){
		PrepareDataForSerialization();
	}

	public void PrepareDataForSerialization(){
		_data.compounds = GetComponentsInChildren<Compound>().Select(x => x.data).ToList();
	}

	public void SetData(BlockData data){
		//Buildings in blocks don't change
		//TODO: Implement
	}

	public void InitializeStructure(){
		_data.compounds.Clear();
		foreach(var c in GetComponentsInChildren<Compound>()){
			c.InitializeStructure();
			_data.compounds.Add(c.data);
		}
	}

}

[XmlType("Block")]
public class BlockData{
	[XmlElement("ID")]
	public int ID = -1;

	[XmlArray("Compounds"), XmlArrayItem("Compound", typeof(CompoundData))]
	public List<CompoundData> compounds;

	public BlockData(){
		compounds = new List<CompoundData>();
	}

	public void Add(CompoundData data){
		compounds.Add(data);
	}
}