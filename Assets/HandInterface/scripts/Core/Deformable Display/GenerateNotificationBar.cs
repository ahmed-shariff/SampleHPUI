using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HPUI.Core
{
    public class GenerateNotificationBar : MonoBehaviour
    {
	private static MeshFilter filter;
	private Texture tex;
	public Mesh mesh
	{
	    get { return filter.mesh; }
	}

	public static GameObject notBar;

	public static List<Vector3> vertices;

	bool isGenerated = false;

	public static bool shouldRender = false;

	public void Start()
	{
	    notBar = GameObject.Find("NotificationBar");
	}

	public void CreateMesh()
	{
	    filter = notBar.GetComponent<MeshFilter>();
	    tex = notBar.GetComponent<MeshRenderer>().material.mainTexture;

	    filter.mesh = GenerateMesh();

	    Debug.Log("yuh");

	    isGenerated = true;
	}

	Mesh GenerateMesh()
	{
	    Mesh mesh = new Mesh();

	    vertices = new List<Vector3>();
	    var normals = new List<Vector3>();
	    var uvs = new List<Vector2>();

	    vertices.Add(notBar.transform.InverseTransformPoint(HandCoordinateGetter.index4.transform.position));
	    vertices.Add(notBar.transform.InverseTransformPoint(HandCoordinateGetter.index1.transform.position));
	    vertices.Add(notBar.transform.InverseTransformPoint(HandCoordinateGetter.middle1.transform.position));
	    vertices.Add(notBar.transform.InverseTransformPoint(HandCoordinateGetter.middle4.transform.position));

	    normals.Add(Vector3.up);
	    normals.Add(Vector3.up);
	    normals.Add(Vector3.up);
	    normals.Add(Vector3.up);



	    uvs.Add(new Vector2(0, 1));
	    uvs.Add(new Vector2(1, 1));
	    uvs.Add(new Vector2(1, 0));
	    uvs.Add(new Vector2(0, 0));

	    var triangles = new List<int>();
	    triangles.AddRange(new List<int>()
			       {
				   0,1,2,
				       0,2,3
				       });

	    mesh.SetVertices(vertices);
	    mesh.SetNormals(normals);
	    mesh.SetUVs(0, uvs);
	    mesh.SetTriangles(triangles, 0);

	    return mesh;
	}

	private void Update()
	{

	    if (isGenerated == true)
	    {
		vertices[0] = notBar.transform.InverseTransformPoint(HandCoordinateGetter.index4.transform.position);
		vertices[1] = notBar.transform.InverseTransformPoint(HandCoordinateGetter.index1.transform.position);
		vertices[2] = notBar.transform.InverseTransformPoint(HandCoordinateGetter.middle1.transform.position);
		vertices[3] = notBar.transform.InverseTransformPoint(HandCoordinateGetter.middle4.transform.position);

		mesh.SetVertices(vertices);

		//if (shouldRender)
		//{
		//    notBar.GetComponent<MeshRenderer>().enabled = true;
		//    GeneratePlaneMesh.display.GetComponent<MeshRenderer>().enabled = false;
		//}
		//else
		//{
		//    notBar.GetComponent<MeshRenderer>().enabled = false;
		//    GeneratePlaneMesh.display.GetComponent<MeshRenderer>().enabled = true;
		//}
	    }
	    notBar.GetComponent<MeshRenderer>().enabled = false;
	    GeneratePlaneMesh.display.GetComponent<MeshRenderer>().enabled = true;
	}
    }
}
