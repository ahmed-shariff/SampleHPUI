using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using System;
using System.Linq;

namespace HPUI.Core
{
    public class BtnMapperStatic : MonoBehaviour
    {
	public GameObject btnPrefab;
	public Transform generatePlaneMeshRoot;
	public HandCoordinateGetter handCoordinateGetter;
	private GeneratePlaneMesh generatePlaneMesh;
	private DeformableMesh deformableMesh;
	private TransformAccessArray btns;

	public bool generatedBtns {get; private set;} = false;
	public float width = 1.0f;
	public float height = 5.0f;

	[SerializeField]
	public Method method = Method.fingerFOR_planer;

	public enum Method
	{
	    fingerFOR_planer,
	    fingerFOR_deformed,
	    palmFOR
	}    

	public Transform transformAnchor;
	public Transform basePosition;
	public Transform palmBase;

	[SerializeField]
	public List<RotatingPair> rotatingPairs;

	private bool processGenerateBtns = false;

	private static Vector3[] vertices;
	private static Vector3[] normals; 
	private Vector3 largestAngle, right, up, drawUp, drawRight, temppos;
	private int maxX, maxY;
	private float gridSize;
	private Vector3 scaleFactor, _scaleFactor;

	private List<ButtonController> btnControllers = new List<ButtonController>();
	public class coord {public int x=0; public int y=0; public float maxX=1; public float maxY=1;}
	public coord currentCoord = new coord();

	bool _inUse = false;
	public bool inUse
	{
	    get
	    {
		return _inUse;
	    }
	    set
	    {
		_inUse = value;
		if (generatedBtns)
		{
		    if (_inUse)
			generatePlaneMesh.gameObject.SetActive(true);
		    else
			generatePlaneMesh.gameObject.SetActive(false);
		}
	    }	
	}
	
	// Start is called before the first frame update
	void Start()
	{
	    generatePlaneMesh = generatePlaneMeshRoot.GetComponentInChildren<GeneratePlaneMesh>();
	    deformableMesh = generatePlaneMesh.GetComponent<DeformableMesh>();
	}

	// Update is called once per frame
	void Update()
	{
	    switch (method)
	    {
		case Method.fingerFOR_deformed:
		    deformableSurfaceHandler();
		    break;
		case Method.fingerFOR_planer:
		case Method.palmFOR:
		    nonDefromedSurfaceHandler();
		    break;
	    }
	}

	void OnDestroy()
	{
	    btns.Dispose();
	}

	void deformableSurfaceHandler()
	{
	    if (generatedBtns)
	    {
		// var a = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		deformableMesh.CreateSplines();
		// var b = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		vertices = generatePlaneMesh.mesh.vertices;
		normals = generatePlaneMesh.mesh.normals;
		right = vertices[10] - vertices[1];
		maxY = vertices.Length - GeneratePlaneMesh.x_divisions;
		maxX = GeneratePlaneMesh.x_divisions;

		currentCoord.maxX = maxX;
		currentCoord.maxY = GeneratePlaneMesh.y_divisions;
	    
		// for (var i = 0; i < vertices.Length; i++)
		// {
		
		// }
		var job = new DeformedBtnLayoutJob()
		    {
			scaleFactor = scaleFactor,
			gridSize = gridSize,
			maxX = maxX,
			maxY = maxY
		    };

		var jobHandle = job.Schedule(btns);
		jobHandle.Complete();
		// var c = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		// Debug.Log(jobHandle.IsCompleted + "------- b-a:  " + (b-a) + "      ------- c-b  " + (c-b) +  "  " + (c-a) + " ---- :"  + ((double)((b-a)/(c-a))).ToString("F6"));
	    }
	    else
	    {
		if (handCoordinateGetter.isCalibrated && generatePlaneMesh.meshGenerated)
		{
		    if(processGenerateBtns)
		    {
			Debug.Log("Generating mesh");
			float yCenterOffset, xCenterOffset;
			generateBtns(generatePlaneMesh.mesh.vertices, generatePlaneMesh.mesh.normals, generatePlaneMesh.transform, out yCenterOffset, out xCenterOffset);
			generatedBtns = true;
			inUse = inUse; //triggering the display
			if (inUse)
			    InteractionManger.instance.getButtons();
		    }
		    else
		    {
			// deformableMesh.CreateSplines();
		    }
		}
	    }
	}

	void nonDefromedSurfaceHandler()
	{
	    if (generatedBtns)
	    {
		if (handCoordinateGetter.isCalibrated)
		    return;
		generatePlaneMeshRoot.transform.position = basePosition.position;
		if (method == Method.fingerFOR_planer)
		{
		    float tempAngle, angle = -180;
		    Vector3 tempVector;
		    foreach(var pairs in rotatingPairs)
		    {
			tempVector = palmBase.InverseTransformPoint(pairs.p2.position);
			tempVector.x = 0;
			if (tempVector.z > 0)
			    continue;
			tempVector = palmBase.TransformPoint(tempVector);
			tempVector = tempVector - basePosition.position;
			tempAngle = Vector3.Angle(palmBase.up, tempVector);
			if (tempAngle > angle)
			{
			    angle = tempAngle;
			    largestAngle = tempVector;
			}
		    }

		    // Since the point being used to calculate the surfaces direction is based on `palmBase`;
		    // This is to make sure the points `largestAngle` is a vector along the `palmBase`
		    if (angle == -180)
		    {
			tempVector = palmBase.InverseTransformPoint(basePosition.up  +  basePosition.position);
			tempVector.x = 0;
			tempVector = palmBase.TransformPoint(tempVector);
			tempVector = tempVector - basePosition.position;
			largestAngle = tempVector;
		    }
	    
		    generatePlaneMeshRoot.transform.rotation = Quaternion.LookRotation(Vector3.Cross(-palmBase.right, largestAngle), largestAngle);
	    
		    vertices = generatePlaneMesh.mesh.vertices;
		    for (var i = 0; i < vertices.Length; i++)
		    {
			btns[i].localPosition = vertices[i];
		    }
		}
		else
		{
		    generatePlaneMeshRoot.transform.rotation = Quaternion.LookRotation(-palmBase.forward, palmBase.up);
		}
	    }
	    else
	    {
		if ( !generatePlaneMesh.meshGenerated && HandCoordinateGetter.startFinished && processGenerateBtns)
		{
		    Debug.Log("Generating mesh");
		    float[] dims = new float[2]{height, width};
		    generatePlaneMesh.CreateFlatMesh(dims);
		    float yCenterOffset, xCenterOffset;
		    generateBtns(generatePlaneMesh.mesh.vertices, generatePlaneMesh.mesh.normals, generatePlaneMesh.transform, out yCenterOffset, out xCenterOffset);
		    generatedBtns = true;
		    InteractionManger.instance.getButtons();
		    generatePlaneMesh.transform.localPosition = transformAnchor.localPosition;
		    generatePlaneMesh.transform.localRotation = transformAnchor.localRotation;
		}
	    }
	}

	public void ContactAction(ButtonController btn)
	{
	    if (btnControllers.Contains(btn))
	    {
		GeneratePlaneMesh.btnIdToxy(btn.id, out currentCoord.x, out currentCoord.y);
	    }
	}
    
	public void generateBtns()
	{
	    processGenerateBtns = true;
	}
    
	void generateBtns(Vector3[] positions, Vector3[] _normals, Transform parent, out float yCenterOffset, out float xCenterOffset)
	{
	    var right = positions[1] - positions[0];
	    GameObject btn;
	    ButtonController btnCtrl;
	    scaleFactor = Vector3.zero;

	    Transform[] _btns = new Transform[positions.Length];

	    btnControllers.Clear();
	
	    for(var i = 0; i < positions.Length; i ++)
	    {
		btn = Instantiate(btnPrefab);
		btn.transform.name = "X" + (int) i % GeneratePlaneMesh.x_divisions + "-Y" + (int) i / GeneratePlaneMesh.x_divisions;
		// Getting the scale values to set the size of the buttons based on the size of a single square in the generated mesh
		if (scaleFactor == Vector3.zero)
		{
		    Vector3 buttonSize = btn.GetComponentInChildren<MeshRenderer>().bounds.size;//.Where(x => x.Zone == ButtonZone.Type.contact).ToList()[0].GetComponent<Collider>();
		    Debug.Log("+++" + parent.InverseTransformVector(Vector3.up * generatePlaneMesh.step_size).magnitude.ToString("F6") + "  " + (positions[0] - positions[1]).magnitude.ToString("F6"));
		    Debug.Log("+++" + buttonSize.ToString("F6") + "       " + btn.GetComponentInChildren<MeshRenderer>().transform.lossyScale.ToString("F6"));
		    // float gridSize = parent.InverseTransformVector(Vector3.up * generatePlaneMesh.step_size).magnitude;
		    gridSize = parent.InverseTransformVector(positions[0] - positions[1]).magnitude;
		
		    scaleFactor = btn.transform.localScale;
		    // making them slightly larger to remove the spaces between the pixels
		    scaleFactor.x = (gridSize / buttonSize.x) * 1.05f * parent.lossyScale.x;
		    scaleFactor.y = (gridSize / buttonSize.y) * 1.05f * parent.lossyScale.y;
		    scaleFactor.z = 1/parent.lossyScale.z;
		    gridSize = (positions[0] - positions[1]).magnitude;
		    Debug.Log("+++" + scaleFactor.ToString("F4"));
		}
		btn.transform.parent = parent;
		btn.transform.localPosition = positions[i];
		btn.transform.localRotation = Quaternion.identity;
		btn.transform.localScale = scaleFactor;// Vector3.one * 0.06f;
		btnCtrl = btn.GetComponentInChildren<ButtonController>();
		btnCtrl.id = i;
		// Debug.Log(btnCtrl + "" +  btns + "  " + btnCtrl.transform.parent);
		_btns[i] = btnCtrl.transform.parent;
	    
		btnControllers.Add(btnCtrl);
		btnCtrl.contactAction.AddListener(ContactAction);
	    }
	    btns = new TransformAccessArray(_btns);
	    var yPos = (from pos in positions select pos[1]);
	    var xPos = (from pos in positions select pos[0]);
	    yCenterOffset = (yPos.Max() - yPos.Min()) / 2;
	    xCenterOffset = (xPos.Max() - xPos.Min()) / 2;
	}

	void OnDrawGizmos()
	{
	    // if (generatePlaneMesh.meshGenerated)
	    // {
	    //     generatePlaneMeshRoot.transform.position = basePosition.position;
	    //     GameObject ob;
	    //     foreach (var v in generatePlaneMesh.mesh.vertices)
	    //     {
	    // 	Gizmos.DrawSphere(v, 0.01f);
	    //     }
	    //     Debug.Log(generatePlaneMesh.mesh.vertices.Length);
	    //     Debug.Break();
	    // }
	    if (generatedBtns)
	    {
		// Gizmos.DrawRay(btns[922].transform.position, drawUp * 200, Color.green);
		// Gizmos.DrawRay(btns[922].transform.position, drawRight * 200, Color.red);
	    }
	}
    
	[Serializable]
	public class RotatingPair{
	    public Transform p1;
	    public Transform p2;
	}

	struct DeformedBtnLayoutJob: IJobParallelForTransform
	{
	    private Vector3 right, up, temppos, _scaleFactor;
	    public Vector3 scaleFactor;
	    public float gridSize; 
	    public int maxX, maxY;
	
	    public void Execute(int i, TransformAccess btn)
	    {
		temppos = BtnMapperStatic.vertices[i];
		temppos.z += -0.0002f;
		btn.localPosition = temppos;
		if (true)//btn.isSelectionBtn)
		{
		    if (i > maxX)
			up = BtnMapperStatic.vertices[i] - BtnMapperStatic.vertices[i - maxX];//up = btn.transform.position - btns[i - maxX].transform.position;
		    else
			up = BtnMapperStatic.vertices[i + maxX] - BtnMapperStatic.vertices[i];
		    
		    if ( i % maxX == 0)
			right = BtnMapperStatic.vertices[i + 1] - BtnMapperStatic.vertices[i];
		    else
			right = BtnMapperStatic.vertices[i] - BtnMapperStatic.vertices[i-1];//right = btn.transform.position - btns[i-1].transform.position;

		    // if (false)//i == 922)
		    // {
		    //     drawUp = up;
		    //     drawRight = right;
		    //     Debug.DrawRay(btn.transform.position, drawUp * 200, Color.green); 
		    //     Debug.DrawRay(btn.transform.position, drawRight * 200, Color.red);
		    //     Debug.Log("------ " + i + "   " + (i + 1));
		    // }
		    
		    btn.localRotation = Quaternion.LookRotation(normals[i], up);//Vector3.Cross(right, up), up);//-btn.transform.forward, up);
		    _scaleFactor.x = (right.magnitude / gridSize) * scaleFactor.x;
		    _scaleFactor.y = (up.magnitude / gridSize) * scaleFactor.y;
		    _scaleFactor.z = scaleFactor.z;
		    btn.localScale = _scaleFactor;
		    // btn.transform.parent.forward = -btn.transform.parent.forward;
		    // btn.transform.parent.forward = -normals[i];
		}
		else
		{
		    btn.localRotation = Quaternion.LookRotation(normals[i]);
		}
	    }
	}
    }
}
