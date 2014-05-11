using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameUtils;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


[ExecuteInEditMode]
public class SceneManagerState : MonoBehaviour {


	public Vector3 floorStandardMeasuresMeters = new Vector3(12,2,12);
	public Vector3 blockMaxAllowedSizeNormalized;
	public Vector3 blockSizeMeters;
	public Vector3 buildingMaxSizeNormalized;
	public Vector3 buildingSizeMeters;
	public Vector2 floorMaxSizeNormalized; 

	private string tooltip = "Esc: Out one context\nLeft Click: In one context";
	private string[] paramKeyNames = new string[]{"Left Shift", "Left Alt", "Left Ctrl"};
	private KeyCode[] paramNumbers = new KeyCode[]{KeyCode.LeftShift, KeyCode.LeftAlt, KeyCode.LeftControl};
	private GUIText paramView;
	IEnumerable<MouseLook> controllers;

	Collider selectedContext;


	XmlSerializer serializer;
	SceneData _data;
	public string fileName;

	public SceneData data {
		get {return _data;}
		set {_data = value;}
	}

	public GameObject context {
		get {return selectedContext.gameObject;}
	}

//	void Awake(){}

	void Start () {
		controllers = FindObjectsOfType<MouseLook>().Where (x => x.CompareTag("MainCamera") || x.CompareTag("Player"));
		paramView = (GameObject.Find("GUI ParamView") as GameObject).guiText;
		selectedContext = GetComponent<Collider>();	
	}
	

	void Update () {
		if (Input.anyKey){
			foreach(var v in controllers){
				v.enabled = false;
			}
		} else {
			foreach(var v in controllers){
				v.enabled = true;
			}
		}

		if(Input.GetMouseButtonDown(0)){
			Pointer2D(Input.mousePosition);
			BroadcastMessage("SetSelection", selectedContext.gameObject);
			Debug.Log("Selected Context: " + selectedContext.gameObject.name);
		} else if (Input.GetKeyDown(KeyCode.Escape)) {
			if (selectedContext.transform.parent.collider){
				selectedContext = selectedContext.transform.parent.collider;
				BroadcastMessage("SetSelection", selectedContext.gameObject);
			}
			Debug.Log("Selected Context: " + selectedContext.gameObject.name);
		}

		for (int i = 0 ; i < paramNumbers.Length ; i++){
			if (Input.GetKey(paramNumbers[i])){
				var v = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
				selectedContext.SendMessage("UpdateParam", new ParamData(i, v));
				Debug.Log("Sent Something: "+ v);
			}
		}
	}

	void Pointer2D(Vector2 point){
		var ray = Camera.main.ScreenPointToRay(point);
		SelectContext(ray);
	}

	void Pointer3D(Vector3 controllerPosition, Transform reference){
		var ray = new Ray(reference.position, controllerPosition - reference.position);
		SelectContext(ray);
	}


	bool SelectContext(Ray ray){
		if (selectedContext) {
			var hits = Physics.RaycastAll(ray);
//			FIXME: Should be the distance of the hit, not the center o the object hitting
			var selection = hits.Where(x => (SeekParent(x.collider.transform, selectedContext.transform) > 0))
				.OrderBy(x => SeekParent(x.collider.transform, selectedContext.transform)).ThenBy(x => x.distance);
			if (selection.Count() > 0){
				selectedContext = selection.First().collider;
				return true;
			}
		}
	return false;
	}

	public void UpdateParamDisplay(string[] names){
		string[] zipped = Functional.Zip(paramKeyNames, names, (a, b) => a + " : " + b).ToArray();
		paramView.text = string.Join("\n", zipped) + "\n" + tooltip;
	}

	private int SeekParent(Transform child, Transform potentialAncestor, int depthSearched=0){
		if (!child.parent){
			return -1;
		} else if (child.parent == potentialAncestor) {
			return depthSearched + 1;
		} else {
			return SeekParent(child.parent, potentialAncestor, depthSearched + 1);
		}
	}


//	public void SetData(SceneData data){
//		try
//	}

	public void LoadXMLScene(string name){
		//TODO: Check if file is saved
		serializer = new XmlSerializer(typeof(SceneData));
		SceneManagerState s;
		using (var stream = new FileStream(Path.Combine(Application.dataPath, name), FileMode.Open)){
			s = serializer.Deserialize (stream) as SceneManagerState;
		}
		//use InstanceIDToObject

	}

	public void SaveXMLScene(){
		if (serializer == null || fileName == null){
			throw new UnityException("Nowhere to save");
		}
		//FIXME: Not sure sbout this
		using(var stream = new FileStream(Path.Combine(Application.dataPath, fileName), FileMode.Create))
		{
			serializer.Serialize(stream, _data);
		}
	}

	public void NewXMLSceneFromState(string name){
		serializer = new XmlSerializer(typeof(SceneData));
		fileName = name;
		using(var stream = new FileStream(Path.Combine(Application.dataPath, name), FileMode.Create))
		{
			serializer.Serialize(stream, _data);
		}
	}	

	public void InitializeStructure(){
		_data = new SceneData();
		foreach (var b in GetComponentsInChildren<Block>()){
			b.InitializeStructure();
			_data.blocks.Add(b.data);
		}
	}

}

//[XmlInclude(typeof(BlockData))]
[XmlRoot("Scene")]
public class SceneData { //: IEnumerable<BlockData>
	[XmlArray("Blocks"), XmlArrayItem("Block", typeof(BlockData))]
	public List<BlockData> blocks;
			
	public SceneData (){
		blocks = new List<BlockData>();
	}

	public void Add(BlockData data){
		blocks.Add(data);
	}

//	public IEnumerator<BlockData> GetEnumerator(){
//		return blocks.GetEnumerator();
//	}

//	public IEnumerator GetEnumerator(){
//		return blocks.GetEnumerator();
//	}

}

