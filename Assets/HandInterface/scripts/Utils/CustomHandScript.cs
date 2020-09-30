using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

//// using ViconDataStreamSDK.CSharp;

// using ViconDataStreamSDK.DotNET;
using Newtonsoft.Json;

namespace HPUI.Utils
{
    public class CustomHandScript : CustomSubjectScript
    {
	public float normalOffset = 0.001f;
	public bool setPosition = true;
	public bool setScale = true;
	private Vector3 normal;
	private Vector3 palm;
	private bool noHand = false;
    

	Dictionary<string, string> segmentChild = new Dictionary<string, string>()
	{
	    //{"Arm", null},
	    {"Arm", "Hand"},
	    {"Hand", "R3D1_2_2"},

	    {"R1D1_2", "R1D2_2"},
	    {"R1D2_2", "R1D3_2"},
	    {"R1D3_2", "R1D4_2"},
        
	    {"R2D1_2", "R2D2_2"},
	    {"R2D2_2", "R2D3_2"},
	    {"R2D3_2", "R2D4_2"},
        
	    {"R3D1_2", "R3D2_2"},
	    {"R3D2_2", "R3D3_2"},
	    {"R3D3_2", "R3D4_2"},
        
	    {"R4D1_2", "R4D2_2"},
	    {"R4D2_2", "R4D3_2"},
	    {"R4D3_2", "R4D4_2"},

	    {"R5D1_2", "R5D2_2"},
	    {"R5D2_2", "R5D3_2"},
	    {"R5D3_2", "R5D4_2"},
	};

	Dictionary<string, string> segmentParents = new Dictionary<string, string>()
	{
	    //{"Arm", null},
	    {"Hand", "Arm"},
	    //{"R1D1_2", "Hand"},
	    //{"R2D1_2", "Hand"},
	    {"R3D1_2", "Hand"},
	    //{"R4D1_2", "Hand"},
	    //{"R5D1_2", "Hand"},

	    {"R1D2_2", "R1D1_2"},
	    {"R2D2_2", "R2D1_2"},
	    {"R3D2_2", "R3D1_2"},
	    {"R4D2_2", "R4D1_2"},
	    {"R5D2_2", "R5D1_2"},
        
	    {"R1D3_2", "R1D2_2"},
	    {"R2D3_2", "R2D2_2"},
	    {"R3D3_2", "R3D2_2"},
	    {"R4D3_2", "R4D2_2"},
	    {"R5D3_2", "R5D2_2"},
        
	    {"R1D4_2", "R1D3_2"},
	    {"R2D4_2", "R2D3_2"},
	    {"R3D4_2", "R3D3_2"},
	    {"R4D4_2", "R4D3_2"},
	    {"R5D4_2", "R5D3_2"},
	};

	void Start()
	{
	    segmentMarkers = new Dictionary<string, List<string>>() {
		{ "Arm", new List<string>() {"RFA2", "RFA1"}} ,//{ "RWRB", "RFA2", "RFA1", "RWRA" } } ,
		{    "Hand", new List<string>() { "RWRB", "RWRA" }} ,//{"RH1", "RH3", "RH6"}}, 
		{    "R1D1_2",  new List<string>(){"RTH1"}},
		{    "R1D2_2", new List<string>(){"RTH2"}},
		{    "R1D3_2", new List<string>(){"RTH3", "RTH3P"}},
		{    "R1D4_2", new List<string>{"RTH4"}},

		{    "R2D1_2", new List<string>{"RH2"}},
		{    "R2D2_2", new List<string>{"RIF1"}}, 
		{    "R2D3_2", new List<string>{"RIF2"}},
		{    "R2D4_2", new List<string>{"RIF3"}}, 
        
		{    "R3D1_2", new List<string>{"RH3"}},
		{    "R3D2_2", new List<string>{"RTF1"}},
		{    "R3D3_2", new List<string>{"RTF2"}},
		{    "R3D4_2", new List<string>{"RTF3"}},
        
		{    "R4D1_2", new List<string>{"RH4"}},
		{    "R4D2_2", new List<string>{"RRF2"}},
		{    "R4D3_2", new List<string>{"RRF3"}},
		{    "R4D4_2", new List<string>{"RRF4"}},
        
		{   "R5D1_2", new List<string>(){"RH5"}}, 
		{    "R5D2_2",  new List<string>(){"RPF1"}}, 
		{    "R5D3_2", new List<string>(){"RPF2"}},
		{    "R5D4_2", new List<string>(){"RPF3"}}
	    };
	    SetupWriter();
	    SetupFilter();
	}
	private Dictionary<string, Vector3> baseVectors = new Dictionary<string, Vector3>();
	private Dictionary<string, Vector3> previousSegments = new Dictionary<string, Vector3>();


	protected override Dictionary<string, Vector3> processSegments(Dictionary<string, Vector3> segments, Data data)
	{
	    palm = segments["Hand"] - (segments["R3D1_2"] + 0.5f * (segments["R4D1_2"] - segments["R4D1_2"]));
	    normal = Vector3.Cross(palm, segments["R4D1_2"] - segments["R3D1_2"]);
	    baseVectors["R1"] = segments[segmentChild["R1D1_2"]] - segments["R1D1_2"];
	    baseVectors["R2"] = segments[segmentChild["R2D1_2"]] - segments["R2D1_2"];
	    baseVectors["R3"] = segments[segmentChild["R3D1_2"]] - segments["R3D1_2"];
	    baseVectors["R4"] = segments[segmentChild["R4D1_2"]] - segments["R4D1_2"];
	    baseVectors["R5"] = segments[segmentChild["R5D1_2"]] - segments["R5D1_2"];

	    // Debug.Log(data.data["RTH3P"] + "  -  "+ data.data["RTH3"]);
	    var p1 = data.data["RTH3P"];
	    var p2 = data.data["RTH3"];
	    baseVectors["R1_right"] = (new Vector3(p2[0], p2[2], p2[1])) - (new Vector3(p1[0], p1[2], p1[1]));
	    // Vector3.Cross(segments["R1D4_2"] - segments["R1D3_2"], segments["R1D3_2"] - segments["R1D2_2"]);
	    // Debug.DrawRay(segments["R3D1_2"], normal);
	    //Debug.Log(normal.magnitude);
	    //Debug.Log((normal * 0.01f).magnitude);
	    segments["PalmBase"] = normal;// *0.01f;
	    if (palm == Vector3.zero)
		noHand = true;
	    else
	    {
		noHand = false;
		transform.root.rotation = Quaternion.LookRotation(-normal, -palm);
	    }
	    return segments;
	}

	private Matrix4x4 handWorldToLocalMatrix;
    
	protected override string constructFinalWriterString()
	{
	    return "[" + base.constructFinalWriterString() + ", [" +
		handWorldToLocalMatrix[0, 0] + ", " + handWorldToLocalMatrix[0, 1] + ", " + handWorldToLocalMatrix[0, 2] + ", " + handWorldToLocalMatrix[0, 3] + ", " +
		handWorldToLocalMatrix[1, 0] + ", " + handWorldToLocalMatrix[1, 1] + ", " + handWorldToLocalMatrix[1, 2] + ", " + handWorldToLocalMatrix[1, 3] + ", " +
		handWorldToLocalMatrix[2, 0] + ", " + handWorldToLocalMatrix[2, 1] + ", " + handWorldToLocalMatrix[2, 2] + ", " + handWorldToLocalMatrix[2, 3] + ", " +
		handWorldToLocalMatrix[3, 0] + ", " + handWorldToLocalMatrix[3, 1] + ", " + handWorldToLocalMatrix[3, 2] + ", " + handWorldToLocalMatrix[3, 3] + ", " +
		"]]";
	}

	protected override void ApplyBoneTransform(Transform Bone)
	{
	    string BoneName = Bone.gameObject.name;

	    //if (segmentParents.ContainsKey(BoneName) && segments.ContainsKey(BoneName))
	    if (segments.ContainsKey(BoneName))
		//if (segmentChild.ContainsKey(BoneName) && segments.ContainsKey(BoneName))
	    {
		Vector3 BonePosition = segments[BoneName];
		bool usePreviousSegments = false;
		// if (BonePosition == Vector3.zero && !noHand)
		// {
		//     if (previousSegments.ContainsKey(BoneName))
		//         BonePosition = previousSegments[BoneName];
		//     usePreviousSegments = true;
		//     Debug.Log(BoneName +"   "+BonePosition + " " + usePreviousSegments + " "+ previousSegments[BoneName]);
		// }
		// else
		// {
		//     previousSegments[BoneName] = BonePosition;
		// }
	    
		//string ParentName = segmentParents[BoneName];
		// string childName = segmentChild[BoneName];
		if (BoneName == "PalmBase")
		{
		    if (!noHand)
			Bone.rotation = Quaternion.LookRotation(-BonePosition.normalized, -palm);
		    Bone.position = Bone.parent.position - Bone.forward * scale_2 - Bone.up * scale_2;
		    // Debug.Log("===========================  " + Bone.position);
		}
		else
		{
		    string fingerId = BoneName.Substring(0, 2);
		    if (setPosition)
		    {
			Bone.position = BonePosition * scale_1; // Bone.parent.InverseTransformPoint();
		    }
		    // if (fingerId)
		    // + normal.normalized * normalOffset
		    if (setScale)
		    {
			Transform p = Bone.parent;
			Bone.parent = null;
			Bone.localScale = Vector3.one * scale_2;
			Bone.parent = p;
		    }
		    // Debug.Log("===========================+++++++++  " + Bone.position);
		    if (!usePreviousSegments)
		    {
			if (segmentChild.ContainsKey(BoneName))
			{
			    if (baseVectors.ContainsKey(fingerId))
			    {
				Vector3 upDirection = segments[segmentChild[BoneName]] - BonePosition;
				if (upDirection != Vector3.zero)
				{
				    Vector3 right;
				    Vector3 forward;
				    if (fingerId == "R1"){
					right = baseVectors["R1_right"];
					//right = Vector3.Cross(normal, baseVectors[fingerId]);
					forward = Vector3.Cross(upDirection, right);
				    }else{
					right = Vector3.Cross(normal, baseVectors[fingerId]);
					forward = Vector3.Cross(upDirection, right);
				    }
				    if (forward != Vector3.zero)
					Bone.rotation = Quaternion.LookRotation(forward, upDirection);
				}
			    }
			} else {
			    // Bone.rotation = Quaternion.identity;
			}
			if (setPosition)
			{
			    if (fingerId == "R1")
				Bone.position += Bone.forward * normalOffset * 0.9f;
			    else if (fingerId == "R3")
				Bone.position += Bone.forward * normalOffset * 1.08f;
			    else if (fingerId == "R4")
				Bone.position += Bone.forward * normalOffset * 1.13f;
			    else if (fingerId == "R5")
				Bone.position += Bone.forward * normalOffset * 1.2f;
			    else
				Bone.position += Bone.forward * normalOffset;
			}
		    }
		}
	    }
	    addBoneDataToWriter(Bone);
	    if (Bone.name == "Hand")
		handWorldToLocalMatrix = Bone.worldToLocalMatrix;
	}
    }
}
