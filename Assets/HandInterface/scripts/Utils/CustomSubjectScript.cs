using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net.Http;
using System;
using System.IO;
using System.Linq;
using HPUI.Core;

//// using ViconDataStreamSDK.CSharp;

// using ViconDataStreamSDK.DotNET;
using Newtonsoft.Json;

namespace HPUI.Utils
{
    public class CustomSubjectScript : MonoBehaviour
    {
	public float scale_1 = 0.001f;
	public float scale_2 = 0.02f;
	public string rootSegment = "Arm";
	public string URI = "http://192.168.0.105:8080/marker/HWD";
	public string defaultData = "{\"data\": {\"RWRB\": [-543.6625324688598, 207.2696870612411, 298.7514053730324], \"RFA2\": [-532.0721277646578, 220.17137432871033, 301.01629761935317], \"RFA1\": [-520.1440660572242, 201.39104705712728, 339.41934514555805], \"RWRA\": [-532.6974300716365, 189.02367806197196, 337.42141847242124], \"RH1\": [-560.7345454383594, 159.62419546128493, 330.3774691418835], \"RH3\": [-590.0032621097643, 131.76129785698242, 299.0094709326491], \"RH6\": [-562.2275968721467, 178.22968321613172, 289.7780921183954], \"RTH1\": [-521.0776063001258, 156.02975240617602, 339.52347728151585], \"RTH2\": [-533.1608764185024, 125.93223864863384, 346.6751934035616], \"RTH3\": [-544.0308683261262, 94.88325770113741, 340.18541871909747], \"RTH3P\": [-558.2383999037787, 98.54284010368167, 344.30830190364924], \"RTH4\": [-565.5234537423078, 83.35392844673802, 323.32104505694093], \"RH2\": [-585.6043517556936, 120.51194616610833, 321.788774764582], \"RIF1\": [-576.5885643353779, 78.82999103456628, 300.3582074396528], \"RIF2\": [-561.2383839807005, 59.365051118922004, 295.3749509588552], \"RIF3\": [-541.7682180796493, 45.73872562944883, 292.4172960222232], \"RTF1\": [-572.5416262201588, 87.20318096871338, 285.1907380694027], \"RTF2\": [-562.4119589663006, 62.738780575654275, 271.7188525984546], \"RTF3\": [-557.640123472733, 48.67593410004015, 265.6647298151231], \"RH4\": [-584.7190340388772, 139.86043222122856, 281.7731182084455], \"RRF1\": [-585.3695333810368, 117.21631146711684, 275.4715366418419], \"RRF2\": [-587.3318441581403, 98.12953148435754, 262.2992975340355], \"RRF3\": [-588.7462283921602, 71.40473234827627, 248.39760510410048], \"RRF4\": [-587.6055606791075, 56.662726798838854, 241.1235659054331], \"RH5\": [-576.2821771931049, 149.15875547966468, 267.5793086555055], \"RPF1\": [-587.1382537041475, 129.22951405026535, 250.65671596147286], \"RPF2\": [-593.5975138169013, 115.06673347598871, 237.79061600397645], \"RPF3\": [-598.0083756011302, 98.17580941339943, 225.52275793399647]}, \"hierachy\": {\"Arm\": [\"RWRB\", \"RFA2\", \"RFA1\", \"RWRA\"], \"Hand\": [\"RH1\", \"RH3\", \"RH6\"], \"R1D1\": [\"RTH1\"], \"R1D2\": [\"RTH2\"], \"R1D3\": [\"RTH3\", \"RTH3P\", \"RTH4\"], \"R2D1\": [\"RH2\"], \"R2D2\": [\"RIF1\"], \"R2D3\": [\"RIF2\", \"RIF3\"], \"R3D2\": [\"RTF1\"], \"R3D3\": [\"RTF2\", \"RTF3\"], \"R4D1\": [\"RH4\", \"RRF1\"], \"R4D2\": [\"RRF2\"], \"R4D3\": [\"RRF3\", \"RRF4\"], \"R5D1\": [\"RH5\"], \"R5D2\": [\"RPF1\"], \"R5D3\": [\"RPF2\", \"RPF3\"]}, \"sensorTriggered\": true}";
	public bool enableWriteData = true;
	private bool processedRequest = true;
	public bool useOneEuroFilter = false;
	private static readonly HttpClient client = new HttpClient();

	private IEnumerator e;

	public Text outputText;

	bool _sensorTriggered;
	public bool sensorTriggered {
	    get
	    {
		return _sensorTriggered;
	    }
	    private set
	    {
		// if (value || expectSensorChange)
		// 	Debug.Log("--  " + expectSensorChange + "      val:: " + value);
		if (!value)
		    expectSensorChange = false;
		//if (!expectSensorChange)
		_sensorTriggered = value && !expectSensorChange;
	    }
	}
	public bool expectSensorChange {get; set;}
	public bool processFrameFlag {get; set;}
    
	Data data;
	string filePath;
	StreamWriter inputWriter;
	StreamWriter finalWriter;
	StreamWriter rawWriter;

	string rawData;
    
	protected Dictionary<string, Vector3> finalPositionVectors = new Dictionary<string, Vector3>();
	protected Dictionary<string, Vector3> finalUpVectors = new Dictionary<string, Vector3>();
	protected Dictionary<string, Vector3> finalForwardVectors = new Dictionary<string, Vector3>();
    
	protected Dictionary<string, Vector3> segments = new Dictionary<string, Vector3>();
	protected Dictionary<string, Quaternion> segmentsRotation = new Dictionary<string, Quaternion>();
	protected Dictionary<string, List<string>> segmentMarkers;
	private Dictionary<string, OneEuroFilter[]> segmentsFilters = new Dictionary<string, OneEuroFilter[]>();
	private Dictionary<string, List<float>> previousData = new Dictionary<string, List<float>>();
    
	private Dictionary<string, Vector3> baseVectors = new Dictionary<string, Vector3>();
    
	//ViconDataStreamSDK_DotNET.Client pHeapClient = new ViconDataStreamSDK.DotNET.Client();
	void Start()
	{
	    segmentMarkers = new Dictionary<string, List<string>>() {
		{ "base1", new List<string>() { "base1"}},
		{ "base2", new List<string>() { "base2"}},
		{ "base3", new List<string>() { "base3"}},
		{ "base4", new List<string>() { "base4"}}
	    };
	    SetupWriter();
	    SetupFilter();
	}

	protected void SetupWriter()
	{
	    if (!enableWriteData)
		return;
	    filePath = getPath("input"); 
	    inputWriter = new StreamWriter(filePath, true);
	    Debug.Log("Writing to:  " + filePath);

	    filePath = getPath("final"); 
	    finalWriter = new StreamWriter(filePath, true);
	    Debug.Log("Writing to:  " + filePath);

	    filePath = getPath("raw"); 
	    rawWriter = new StreamWriter(filePath, true);
	    Debug.Log("Writing to:  " + filePath);
	}

	protected void SetupFilter()
	{
	    foreach (KeyValuePair<string, List<string>> segment in segmentMarkers)
	    {
		OneEuroFilter[] l = new OneEuroFilter[7];
		for (var i = 0; i < 7; i++)
		{
		    l[i] = new OneEuroFilter(50.0f, 1.0f, 110f, 0.001f);
		}
		segmentsFilters[segment.Key] = l;
	    }
	}

	private string getPath(string suffix)
	{
	    // #if UNITY_EDITOR
	    //         return Application.dataPath + "/Data/"  + "Saved_Inventory.csv";
	    //         //"Participant " + "   " + DateTime.Now.ToString("dd-MM-yy   hh-mm-ss") + ".csv";
	    // #elif UNITY_ANDROID
	    return UnityEngine.Application.persistentDataPath + "/stream_" + suffix + "_" + this.transform.name + "_" + SceneProperties.instance.initTicks + ".csv";
	    // #elif UNITY_IPHONE
	    //         return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
	    // #else
	    //         return Application.dataPath +"/"+"Saved_Inventory.csv";
	    // #endif
	}

	public void writeData()
	{
	    if (!enableWriteData || SceneProperties.instance.useDefaultData)
		return;
	    // Debug.Log(string.Join(",", finalPositionQuaternion.Select(kvp => "[" +kvp.Key + ", " + kvp.Value.ToString("F4") + "]")));
	    inputWriter.WriteLine(SceneProperties.instance.currentTicks + ", " + "{" + string.Join(",", segments.Select(kvp => "[" +kvp.Key + ", " + kvp.Value.ToString("F6") + "]")) + "}");
	    inputWriter.Flush();

	    finalWriter.WriteLine(SceneProperties.instance.currentTicks + ", " +
				  constructFinalWriterString());
	    finalWriter.Flush();

	    rawWriter.WriteLine(SceneProperties.instance.currentTicks + ", " + rawData);
	    rawWriter.Flush();
	}

	protected virtual string constructFinalWriterString()
	{
	    return "{ 'positions':" + string.Join(",", finalPositionVectors.Select(kvp => "[" +kvp.Key + ", " + kvp.Value.ToString("F6") + "]")) +
		", 'up':" + string.Join(",", finalUpVectors.Select(kvp => "[" +kvp.Key + ", " + kvp.Value.ToString("F6") + "]")) +
		", 'forward':" + string.Join(",", finalForwardVectors.Select(kvp => "[" +kvp.Key + ", " + kvp.Value.ToString("F6") + "]")) +
		"}";
	}

	void Update()
	{
	    if (processedRequest){
		processedRequest = false;
		if (SceneProperties.instance.useDefaultData)
		{
		    processData(defaultData);
		    processedRequest = true;
		    processFrameFlag = true;
		}
		else
		{
		    StartCoroutine(GetRequestUnityClient(URI));
		}
		// GetRequestHttpClient(URI);
	    }
	}

	IEnumerator GetRequestUnityClient(string uri)
	{
	    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
	    {
		// Request and wait for the desired page.
		yield return webRequest.SendWebRequest();
		string[] pages = uri.Split('/');
		int page = pages.Length - 1;
		if (webRequest.isNetworkError)
		{
		    Debug.Log(pages[page] + ": Error: " + webRequest.error);
		    Debug.Log(pages[page] + ": Error: " + uri);
		}
		else
		{
		    // Debug.Log(data.sensorTriggered.ToString());

		    try{
			processData(webRequest.downloadHandler.text);
		    } catch(Exception err){
			Debug.Log("Exception: " + err.ToString());
			Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
		    }   
		}
		processedRequest = true;
		processFrameFlag = true;
	    }
	}

	async void GetRequestHttpClient(string uri)
	{
	
	    string content = await client.GetStringAsync(uri);
	    Debug.Log(uri + "\nReceived: " + content);
	    processData(content);
	    processedRequest = true;
	}

	void processData(string inputText)
	{
	    rawData = inputText;
	    Data data = JsonConvert.DeserializeObject<Data>(inputText);
	    sensorTriggered = data.sensorTriggered;
	
	    if (outputText)
	    {
		outputText.text = data.sensorTriggered.ToString();
		outputText.color = data.sensorTriggered ? Color.red: Color.blue;
	    }
	    foreach (KeyValuePair<string, List<string>> segment in segmentMarkers)
	    {
		// do something with entry.Value or entry.Key
		Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
		//
		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		foreach (string marker in segment.Value)
		{
		    var _data = data.data[marker];
		    if (_data[0] == 0 && previousData.ContainsKey(marker))
		    {
			_data = previousData[marker];
		    }
		    else
		    {
			if (useOneEuroFilter)
			{
			    var _filter = segmentsFilters[segment.Key];
			    for(var i = 0; i < _data.Count; i++)
			    {
				_data[i] = _filter[i].Filter(_data[i]);
			    }
			}
			previousData[marker] = _data;
		    }
		    data.data[marker] = _data;
		    List<float> _pos = data.data[marker];
		    pos.x += _pos[0];
		    pos.y += _pos[2];
		    pos.z += _pos[1];
		    //break;
		    if (_pos.Count > 3)
		    {
			rot.x = _pos[3];
			rot.y = _pos[4];
			rot.z = _pos[5];
			rot.w = _pos[6];
		    }
		}
		segments[segment.Key] = pos/segment.Value.Count;
		segmentsRotation[segment.Key] = rot;
	    }
	    segments = processSegments(segments, data);
	    // inputWriter.WriteLine(SceneProperties.currentTicks + ", " + "{" + string.Join(",", segments) + "}");
	    // inputWriter.Flush();
	    transform.root.position = segments[rootSegment] *scale_1;
	    FindAndTransform(transform.root, rootSegment);
	}

	protected virtual Dictionary<string, Vector3> processSegments(Dictionary<string, Vector3> segments, Data data)
	{
	    return segments;
	}

	protected void FindAndTransform(Transform iTransform, string BoneName)
	{
	    int ChildCount = iTransform.childCount;
	    for (int i = 0; i < ChildCount; ++i)
	    {
		Transform Child = iTransform.GetChild(i);
		if (Child.name == BoneName)
		{
		    this.ApplyBoneTransform(Child);
		    TransformChildren(Child);
		    break;
		}
		// if not finding root in this layer, try the children
		FindAndTransform(Child, BoneName);
	    }
	}

	protected void TransformChildren(Transform iTransform)
	{
	    int ChildCount = iTransform.childCount;
	    for (int i = 0; i < ChildCount; ++i)
	    {
		Transform Child = iTransform.GetChild(i);
		this.ApplyBoneTransform(Child);
		TransformChildren(Child);
	    }
	}

	protected virtual void ApplyBoneTransform(Transform Bone)
	{
	    string BoneName = Bone.gameObject.name;
	    //if (segmentParents.ContainsKey(BoneName) && segments.ContainsKey(BoneName))
	    if (segments.ContainsKey(BoneName))
		//if (segmentChild.ContainsKey(BoneName) && segments.ContainsKey(BoneName))
	    {
		Bone.position = segments[BoneName] * scale_1;
		Bone.rotation = segmentsRotation[BoneName];
		// Debug.Log(")))))))))))))))))))))))))))))))))))) " + Bone.position);
	    }
	    addBoneDataToWriter(Bone);
	}

	protected void addBoneDataToWriter(Transform Bone)
	{
	    finalPositionVectors[Bone.name] = Bone.position;
	    // finalPositionQuaternion[Bone.name] = Bone.rotation;
	    finalUpVectors[Bone.name] = Bone.up;
	    finalForwardVectors[Bone.name] = Bone.forward;
	}

	protected class Data
	{
	    public Dictionary<string, List<float>> data;
	    public Dictionary<string, List<string>> hierachy;
	    public bool sensorTriggered;
	}

	void OnDestroy()
	{
	    if (enableWriteData)
	    {
		Debug.Log("Closing file: " + filePath);
		if (inputWriter != null)
		    inputWriter.Close();
		if (finalWriter != null)
		    finalWriter.Close();
		if (rawWriter != null)
		    rawWriter.Close();
	    }
	}
    }
}
