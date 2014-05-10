using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;

[XmlArrayItem("Compund")]
public class Compound : MonoBehaviour {

	[XmlElement("Sidewalk")]
	public Vector4 sidewalk;

	[XmlArray("Floors")]
	List<Floor> floors;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
