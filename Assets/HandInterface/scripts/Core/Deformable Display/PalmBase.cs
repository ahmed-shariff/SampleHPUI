using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPUI.Core
{
    public class PalmBase : MonoBehaviour
    {
	public static GameObject palmBase;

	bool test = false;

	GameObject sphere1;
	GameObject sphere2;
	GameObject sphere3;
	GameObject sphere4;
	GameObject sphere5;
	GameObject sphere6;

	GameObject xaxis;
	GameObject yaxis;
	GameObject zaxis;

	private void Start()
	{
	    palmBase = GameObject.Find("PalmBase");

	    if (test == true)
	    {
		sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere1.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere2.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		sphere3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere3.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		sphere4 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere4.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		sphere5 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere5.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		sphere6 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere6.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);

		xaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		xaxis.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		xaxis.GetComponent<Renderer>().material.color = Color.red;

		yaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		yaxis.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		yaxis.GetComponent<Renderer>().material.color = Color.green;

		zaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		zaxis.transform.localScale += new Vector3(-0.98f, -.98f, -.98f);
		zaxis.GetComponent<Renderer>().material.color = Color.blue;
	    }
	}


	public static Vector3 CoordinatesInPalmReferenceFrame(Vector3 worldCoordinates)
	{
	    //worldCoordinates = HandCoordinateGetter.HandToWorldCoords(worldCoordinates);
	    worldCoordinates = palmBase.transform.InverseTransformPoint(worldCoordinates);
	    return worldCoordinates;
	}

	public static Vector3 PalmToWorldCoords(Vector3 palmCoords)
	{
	    return palmBase.transform.TransformPoint(palmCoords);
	}

	private void Update()
	{
	    //sphere1.transform.position = PalmToWorldCoords(HandCoordinateGetter.keypoints[0]);
	    //sphere2.transform.position = PalmToWorldCoords(HandCoordinateGetter.keypoints[3]);
	    //sphere3.transform.position = PalmToWorldCoords(HandCoordinateGetter.keypoints[12]);
	    //sphere4.transform.position = PalmToWorldCoords(HandCoordinateGetter.keypoints[15]);

	    if (test == true)
	    {
		sphere1.transform.position = PalmToWorldCoords(new Vector3((float)(HandCoordinateGetter.xDifferenceVectors[0, 0] + HandCoordinateGetter.xDifferenceVectors[0, 2]),
									   (float)(HandCoordinateGetter.yDifferenceVectors[0, 1] + HandCoordinateGetter.yDifferenceVectors[0, 2]),
									   (float)(HandCoordinateGetter.calibrationKeypoints[0].z + HandCoordinateGetter.zDifferenceVectors[0, 2])));
		sphere2.transform.position = PalmToWorldCoords(new Vector3((float)(HandCoordinateGetter.xDifferenceVectors[3, 0] + HandCoordinateGetter.xDifferenceVectors[3, 2]),
									   (float)(HandCoordinateGetter.yDifferenceVectors[3, 1] + HandCoordinateGetter.yDifferenceVectors[3, 2]),
									   (float)(HandCoordinateGetter.calibrationKeypoints[3].z + HandCoordinateGetter.zDifferenceVectors[3, 2])));
		sphere3.transform.position = PalmToWorldCoords(new Vector3((float)(HandCoordinateGetter.xDifferenceVectors[12, 0] + HandCoordinateGetter.xDifferenceVectors[12, 2]),
									   (float)(HandCoordinateGetter.yDifferenceVectors[12, 1] + HandCoordinateGetter.yDifferenceVectors[12, 2]),
									   (float)(HandCoordinateGetter.calibrationKeypoints[12].z + HandCoordinateGetter.zDifferenceVectors[12, 2])));
		sphere4.transform.position = PalmToWorldCoords(new Vector3((float)(HandCoordinateGetter.xDifferenceVectors[15, 0] + HandCoordinateGetter.xDifferenceVectors[15, 2]),
									   (float)(HandCoordinateGetter.yDifferenceVectors[15, 1] + HandCoordinateGetter.yDifferenceVectors[15, 2]),
									   (float)(HandCoordinateGetter.calibrationKeypoints[15].z + HandCoordinateGetter.zDifferenceVectors[15, 2])));

		sphere5.transform.position = PalmToWorldCoords(HandCoordinateGetter.calibrationKeypoints[3]);
		sphere6.transform.position = PalmToWorldCoords(HandCoordinateGetter.calibrationKeypoints[15]);

		xaxis.transform.position = PalmToWorldCoords(new Vector3(0, 0, 0));
		xaxis.transform.LookAt(PalmToWorldCoords(new Vector3(1, 0, 0)));

		yaxis.transform.position = PalmToWorldCoords(new Vector3(0, 0, 0));
		yaxis.transform.LookAt(PalmToWorldCoords(new Vector3(0, 1, 0)));

		zaxis.transform.position = PalmToWorldCoords(new Vector3(0, 0, 0));
		zaxis.transform.LookAt(PalmToWorldCoords(new Vector3(0, 0, 1)));
	    }

        
	}

    }
}
