using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HPUI.Core.DeformableSurfaceDisplay
{
    /*
      This class modifies the coordnates of a mesh based on some criteria
     */
    [DefaultExecutionOrder(-120)]
    public class DeformationCoordinateManager : MonoBehaviour
    {
	public bool useSendMessage = true;
	//public static GameObject hand;

	public GameObject index1;
	public GameObject index2;
	GameObject index3;
	public GameObject index4;

	public GameObject middle1;
	GameObject middle2;
	GameObject middle3;
	public GameObject middle4;

	public GameObject ring1;
	GameObject ring2;
	GameObject ring3;
	GameObject ring4;

	public GameObject pinky1;
	GameObject pinky2;
	GameObject pinky3;
	GameObject pinky4;

	public GameObject palmBottom;

	//order of keypoints is as follows:
	//index 1234 middle 1234 ring 1234 pinky 1234 bottom of palm
	public List<Vector3> keypoints;
	public List<Vector3> calibrationKeypoints;
	public List<Vector3> keypointDifferences;
	private List<GameObject> keypointObjects;

	public List<Vector3> undeformedVerticesCoordinates = new List<Vector3>();

	//public List<Vector3> xDifferenceVectors = new List<Vector3>();
	//public List<Vector3> yDifferenceVectors = new List<Vector3>();
	//public List<Vector3> zDifferenceVectors = new List<Vector3>();
	public double[,] xDifferenceVectors;
	public double[,] yDifferenceVectors;
	public double[,] zDifferenceVectors;
	public double[,] xyzDifferenceVectors;

	//indices of POI for computing calibration width/height of display
	// int middleFingerTipIndex = 7;
	// int palmBaseIndex = 16;
	// int indexFingerTipIndex = 3;
	// int pinkyTipIndex = 15;
	// int middleFingerBottomIndex = 4;

	public bool isCalibrated = false;

	public bool startFinished = false;

	float fingerThreshold;
	bool indexIsStraight;
	bool middleIsStraight;
	bool notificationWidthSatisfied;

	Vector3 index;
	Vector3 indexSmall;
	Vector3 middle;
	Vector3 middleSmall;

	public bool useStaticDisplaySize = true;

	public GeneratePlaneMesh generatePlaneMesh;
    
	void Start()
	{
	    isCalibrated = false;
	    keypoints = new List<Vector3>();
	    keypointObjects = new List<GameObject>();
	    calibrationKeypoints = new List<Vector3>();
	    keypointDifferences = new List<Vector3>();

	    //hand = GameObject.Find("b_r_wrist");

	    //find keypoint objects on the hand and add them to ordered list


	    index1 = GameObject.Find("R2D1_anchor");
	    keypointObjects.Add(index1);
	    index2 = GameObject.Find("R2D2_anchor");
	    keypointObjects.Add(index2);
	    index3 = GameObject.Find("R2D3_anchor");
	    keypointObjects.Add(index3);
	    index4 = GameObject.Find("R2D4_anchor");
	    keypointObjects.Add(index4);

	    middle1 = GameObject.Find("R3D1_anchor");
	    keypointObjects.Add(middle1);
	    middle2 = GameObject.Find("R3D2_anchor");
	    keypointObjects.Add(middle2);
	    middle3 = GameObject.Find("R3D3_anchor");
	    keypointObjects.Add(middle3);
	    middle4 = GameObject.Find("R3D4_anchor");
	    keypointObjects.Add(middle4);
	
	    ring1 = GameObject.Find("R4D1_anchor");
	    keypointObjects.Add(ring1);
	    ring2 = GameObject.Find("R4D2_anchor");
	    keypointObjects.Add(ring2);
	    ring3 = GameObject.Find("R4D3_anchor");
	    keypointObjects.Add(ring3);
	    ring4 = GameObject.Find("R4D4_anchor");
	    keypointObjects.Add(ring4);

	    // pinky1 = GameObject.Find("R5D1_anchor");
	    // keypointObjects.Add(pinky1);
	    // pinky2 = GameObject.Find("R5D2_anchor");
	    // keypointObjects.Add(pinky2);
	    // pinky3 = GameObject.Find("R5D3_anchor");
	    // keypointObjects.Add(pinky3);
	    // pinky4 = GameObject.Find("R5D4_anchor");
	    // keypointObjects.Add(pinky4);	


	    // index1 = GameObject.Find("R2D1_anchor_2");
	    // keypointObjects.Add(index1);
	    // index2 = GameObject.Find("R2D2_anchor_2");
	    // keypointObjects.Add(index2);
	    // index3 = GameObject.Find("R2D3_anchor_2");
	    // keypointObjects.Add(index3);
	    // index4 = GameObject.Find("R2D4_anchor_2");
	    // keypointObjects.Add(index4);

	    // middle1 = GameObject.Find("R3D1_anchor_2");
	    // keypointObjects.Add(middle1);
	    // middle2 = GameObject.Find("R3D2_anchor_2");
	    // keypointObjects.Add(middle2);
	    // middle3 = GameObject.Find("R3D3_anchor_2");
	    // keypointObjects.Add(middle3);
	    // middle4 = GameObject.Find("R3D4_anchor_2");
	    // keypointObjects.Add(middle4);
	
	    // ring1 = GameObject.Find("R4D1_anchor_2");
	    // keypointObjects.Add(ring1);
	    // ring2 = GameObject.Find("R4D2_anchor_2");
	    // keypointObjects.Add(ring2);
	    // ring3 = GameObject.Find("R4D3_anchor_2");
	    // keypointObjects.Add(ring3);
	    // ring4 = GameObject.Find("R4D4_anchor_2");
	    // keypointObjects.Add(ring4);

	    // pinky1 = GameObject.Find("R5D1_anchor_2");
	    // keypointObjects.Add(pinky1);
	    // pinky2 = GameObject.Find("R5D2_anchor_2");
	    // keypointObjects.Add(pinky2);
	    // pinky3 = GameObject.Find("R5D3_anchor_2");
	    // keypointObjects.Add(pinky3);
	    // pinky4 = GameObject.Find("R5D4_anchor_2");
	    // keypointObjects.Add(pinky4);
	
	
	    index1 = GameObject.Find("R2D1");
	    //keypointObjects.Add(index1);
	    index2 = GameObject.Find("R2D2");
	    //keypointObjects.Add(index2);
	    index3 = GameObject.Find("R2D3");
	    //keypointObjects.Add(index3);
	    index4 = GameObject.Find("R2D4");
	    //keypointObjects.Add(index4);

	    middle1 = GameObject.Find("R3D1");
	    //keypointObjects.Add(middle1);
	    middle2 = GameObject.Find("R3D2");
	    //keypointObjects.Add(middle2);
	    middle3 = GameObject.Find("R3D3");
	    //keypointObjects.Add(middle3);
	    //middle4 = GameObject.Find("R3D4");
	    //keypointObjects.Add(middle4);

	    ring1 = GameObject.Find("R4D1");
	    //keypointObjects.Add(ring1);
	    ring2 = GameObject.Find("R4D2");
	    //keypointObjects.Add(ring2);
	    ring3 = GameObject.Find("R4D3");
	    //keypointObjects.Add(ring3);
	    ring4 = GameObject.Find("R4D4");
	    //keypointObjects.Add(ring4);

	    pinky1 = GameObject.Find("R5D1");
	    //keypointObjects.Add(pinky1);
	    pinky2 = GameObject.Find("R5D2");
	    //keypointObjects.Add(pinky2);
	    pinky3 = GameObject.Find("R5D3");
	    //keypointObjects.Add(pinky3);
	    pinky4 = GameObject.Find("R5D4");
	    //keypointObjects.Add(pinky4);
	
	    palmBottom = GameObject.Find("PalmBase");
	    keypointObjects.Add(palmBottom);

	    //Debug.Log(palmBottom.transform.position + " " + PalmBase.CoordinatesInPalmReferenceFrame(palmBottom.transform.position));
	    //Debug.Log(middle1.transform.position + " " + PalmBase.CoordinatesInPalmReferenceFrame(middle1.transform.position));
	    //Debug.Log(middle4.transform.position + " " + PalmBase.CoordinatesInPalmReferenceFrame(middle4.transform.position));

	    if (useSendMessage)
		GenerateNotificationBar.notBar.SendMessage("CreateMesh");

	    startFinished = true;
	
	    //Calibrate();
	}

	public void Calibrate()
	{
	    // Debug.Log("aaaaaaa");
	    if (true)//isCalibrated == false)
	    {
		if (DeformableMesh.method != "rbf2")
		{
		    xDifferenceVectors = new double[keypointObjects.Count, 3];
		    yDifferenceVectors = new double[keypointObjects.Count, 3];
		    zDifferenceVectors = new double[keypointObjects.Count, 3];
		}
		else
		{
		    xyzDifferenceVectors = new double[keypointObjects.Count, 5];
		}

		for (int i = 0; i < keypointObjects.Count; i++)
		{
		    calibrationKeypoints.Add(PalmBase.CoordinatesInPalmReferenceFrame(keypointObjects[i].transform.position));
		    keypoints.Add(PalmBase.CoordinatesInPalmReferenceFrame(keypointObjects[i].transform.position));
		    keypointDifferences.Add(keypoints[i] - calibrationKeypoints[i]);

		    //xDifferenceVectors.Add(new Vector3(calibrationKeypoints[i].x, calibrationKeypoints[i].y, keypointDifferences[i].x));
		    //yDifferenceVectors.Add(new Vector3(calibrationKeypoints[i].x, calibrationKeypoints[i].y, keypointDifferences[i].y));
		    //zDifferenceVectors.Add(new Vector3(calibrationKeypoints[i].x, calibrationKeypoints[i].y, keypointDifferences[i].z));
		    if (DeformableMesh.method != "rbf2")
		    {
			xDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			xDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			xDifferenceVectors[i, 2] = keypointDifferences[i].x;

			yDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			yDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			yDifferenceVectors[i, 2] = keypointDifferences[i].y;

			zDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			zDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			zDifferenceVectors[i, 2] = keypointDifferences[i].z;
		    }
		    else
		    {
			xyzDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			xyzDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			xyzDifferenceVectors[i, 2] = keypointDifferences[i].x;
			xyzDifferenceVectors[i, 3] = keypointDifferences[i].y;
			xyzDifferenceVectors[i, 4] = keypointDifferences[i].z;
		    }

		}

		//0: height, 1: width
		float[] dimensions = new float[2];
		var handCoordinateGetter = FindObjectsOfType<HandCoordinateGetter>();
		if (handCoordinateGetter.Length != 1)
		    Debug.LogError("There must be 1 HandCoordinateGetter; but have " + handCoordinateGetter.Length + " in the scene.");
	    
		if (handCoordinateGetter[0].useStaticDisplaySize)
		{
		    var btnMapperStatic = FindObjectsOfType<BtnMapperStatic>();
		    if (btnMapperStatic.Length != 1)
			Debug.LogError("There must be 1 BtnMapperStatic; but have " + btnMapperStatic.Length + " in the scene.");
		    //dimensions[0] = Vector3.Distance(calibrationKeypoints[middleFingerTipIndex], calibrationKeypoints[palmBaseIndex]);
		    dimensions[0] = btnMapperStatic[0].height; //Vector3.Distance(calibrationKeypoints[middleFingerTipIndex], calibrationKeypoints[palmBaseIndex]);
		    dimensions[1] = btnMapperStatic[0].width; //Vector3.Distance(calibrationKeypoints[indexFingerTipIndex], calibrationKeypoints[pinkyTipIndex]);
		}
		else
		{
		    var generatePlaneMesh = FindObjectsOfType<GeneratePlaneMesh>();
		    if (generatePlaneMesh.Length != 1)
			Debug.LogError("There must be 1 GeneratePlaneMesh; but have " + generatePlaneMesh.Length + " in the scene.");
		    var height = Vector3.Distance(index2.transform.position, index4.transform.position) * 1.2f;
		    var width = (height / generatePlaneMesh[0].inputYDivisions) * generatePlaneMesh[0].inputYDivisions * 1.61f;
		    dimensions = new float[] {height, width};
		    // calibrationKeypoints[middleFingerTipIndex], calibrationKeypoints[palmBaseIndex]);
		    // dimensions[1] = Vector3.Distance(index1.transform.position, ring1.transform.position) * 1.9f;
		    // calibrationKeypoints[indexFingerTipIndex], calibrationKeypoints[pinkyTipIndex]);
		}
	    
		Debug.Log(dimensions[0] + " " + dimensions[1]);

		generatePlaneMesh.CreateFlatMesh(dimensions);

		undeformedVerticesCoordinates.Clear();
		for (int i = 0; i < GeneratePlaneMesh.vertices.Count; i++)
		{
		    undeformedVerticesCoordinates.Add(PalmBase.CoordinatesInPalmReferenceFrame(GeneratePlaneMesh.displayToWorldCoords(new Vector3(GeneratePlaneMesh.vertices[i].x, GeneratePlaneMesh.vertices[i].y, GeneratePlaneMesh.vertices[i].z))));
		}

		// GenerateNotificationBar.notBar.SendMessage("CreateMesh");

		Debug.Log("Vertices Count = " + undeformedVerticesCoordinates.Count);
	    
		GeneratePlaneMesh.display.SendMessage("MeshRegenerated");
		isCalibrated = true;
	    }
        
	}

	void Update()
	{
	    if (isCalibrated == true)
	    {
		//Debug.Log(keypointObjects.Count + " " + keypoints.Count);

		//index2.transform.Rotate(new Vector3(index2.transform.rotation.x, index2.transform.rotation.y+1, index2.transform.rotation.z));

		//every frame update the coordinates of all keypoints based on transform.position of the corresponding gameobject
		for (int i = 0; i < keypointObjects.Count; i++)
		{
		    keypoints[i] = PalmBase.CoordinatesInPalmReferenceFrame(keypointObjects[i].transform.position);

		    keypointDifferences[i] = keypoints[i] - calibrationKeypoints[i];
                

		    //list of vectors that will be used to create the 3 splines per frame
		    //contain the calibration x/y coordinates, and an x, y, or z displacement
		    if(DeformableMesh.method != "rbf2")
		    {
			xDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			xDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			xDifferenceVectors[i, 2] = keypointDifferences[i].x;

			yDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			yDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			yDifferenceVectors[i, 2] = keypointDifferences[i].y;

			zDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			zDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			zDifferenceVectors[i, 2] = keypointDifferences[i].z;
		    }
		    else
		    {
			xyzDifferenceVectors[i, 0] = calibrationKeypoints[i].x;
			xyzDifferenceVectors[i, 1] = calibrationKeypoints[i].y;
			xyzDifferenceVectors[i, 2] = keypointDifferences[i].x;
			xyzDifferenceVectors[i, 3] = keypointDifferences[i].y;
			xyzDifferenceVectors[i, 4] = keypointDifferences[i].z;
		    }
  
		}



		//check angles here
		//angle between index 1/2 and 1/4 must be small
		//angle between middle 1/2 and 1/4 must be small
		//angle between index 1/4 and middle 1/4 along certain axis must be > threshold
		fingerThreshold = 30;
		indexIsStraight = false;
		middleIsStraight = false;
		notificationWidthSatisfied = false;

		index = index4.transform.position - index1.transform.position;
		indexSmall = index2.transform.position - index1.transform.position;

		if (System.Math.Abs(Vector3.Angle(index, indexSmall)) < fingerThreshold)
		{
		    indexIsStraight = true;
		}

		middle = middle4.transform.position - middle1.transform.position;
		middleSmall = middle2.transform.position - middle1.transform.position;

		if (System.Math.Abs(Vector3.Angle(middle, middleSmall)) < fingerThreshold)
		{
		    middleIsStraight = true;
		}

		//float xBottomDistance = System.Math.Abs(PalmBase.CoordinatesInPalmReferenceFrame(index1.transform.position).x - PalmBase.CoordinatesInPalmReferenceFrame(middle1.transform.position).x);
		float xTopDistance = System.Math.Abs(PalmBase.CoordinatesInPalmReferenceFrame(index4.transform.position).x - PalmBase.CoordinatesInPalmReferenceFrame(middle4.transform.position).x);
		float xTopDistance2 = System.Math.Abs(PalmBase.CoordinatesInPalmReferenceFrame(ring4.transform.position).x - PalmBase.CoordinatesInPalmReferenceFrame(middle4.transform.position).x);
		float xTopDistance3 = System.Math.Abs(PalmBase.CoordinatesInPalmReferenceFrame(ring4.transform.position).x - PalmBase.CoordinatesInPalmReferenceFrame(pinky4.transform.position).x);


		if (xTopDistance > 3.5 * xTopDistance2 && xTopDistance > 3.5 * xTopDistance3)
		{
		    notificationWidthSatisfied = true;
		}

		if (indexIsStraight && middleIsStraight && notificationWidthSatisfied)
		{
		    GenerateNotificationBar.shouldRender = true;
		}
		else
		{
		    GenerateNotificationBar.shouldRender = false;
		}

		// Debug.Log("ttttttttt "+indexIsStraight + " " + middleIsStraight + " " + notificationWidthSatisfied+" "+ xTopDistance+" "+ xTopDistance2);

	    }
	    //Debug.Log("kpd: " + keypointDifferences[3].x + " " + keypointDifferences[3].y + " " + keypointDifferences[3].z);

	}

	//public static Vector3 HandToWorldCoords(Vector3 handCoords)
	//{
	//    return hand.transform.TransformPoint(handCoords);
	//}

	public Vector3 MidpointCalculation(Vector3 point1, Vector3 point2)
	{
	    return ((point1 + point2) / 2);
	    //Vector3 returnVector;
	    //returnVector = point1 + point2;
	    //returnVector.x = returnVector.x / 2f;
	    //returnVector.y = returnVector.y / 2f;
	    //returnVector.z = returnVector.z / 2f;
	    //return returnVector;
	}
    }
}
