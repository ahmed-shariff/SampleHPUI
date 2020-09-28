using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR) 
using UnityEditor;
#endif

public class CreateRegion : MonoBehaviour
{
    public GameObject targetObject;
    public CustomSubjectScript customSubjectHandler;
    public Transform baseObject;
    public string prefabName;

    private Vector3 _scale;
    private List<GameObject> meshFilters;

    private bool recordObjects = false;
    private string path;
    // Start is called before the first frame update
    void Start()
    {
#if (UNITY_EDITOR) 
        var p = targetObject.transform.parent;
	targetObject.transform.parent = null;
	_scale = targetObject.transform.localScale;
	targetObject.transform.parent = p;
	meshFilters = new List<GameObject>();
	path = "Assets/HandInterface/prefabs/data/" + prefabName + ".prefab";
	var ob = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
	if (ob != null)
	{
	    Debug.LogError("Prefab already exists");
	    EditorApplication.isPlaying = false;
	}

#endif
    }

    // Update is called once per frame
    void Update()
    {
#if (UNITY_EDITOR) 
        if (customSubjectHandler.processFrameFlag && recordObjects)
	{
	    var ob = Instantiate(targetObject.gameObject, targetObject.transform.position, targetObject.transform.rotation);
	    ob.transform.localScale = _scale;
	    ob.transform.parent = baseObject;
	    Destroy(ob.GetComponent<CreateRegion>());
	    DestroyChildren(ob.transform);
	    ob.GetComponent<MeshRenderer>().enabled = true;
	    
	    meshFilters.Add(ob);

	    customSubjectHandler.processFrameFlag = false;
	}
    }

    void DestroyChildren(Transform transform) {
	for (int i = transform.childCount - 1; i >= 0; --i) {
	    GameObject.Destroy(transform.GetChild(i).gameObject);
	}
	transform.DetachChildren();
#endif
    }

    public void ButtonStart()
    {
	recordObjects = true;
    }

    public void ButtonStop()
    {
#if (UNITY_EDITOR)

	recordObjects = false;
	Debug.Log(meshFilters.Count);
	CombineInstance[] combine = new CombineInstance[meshFilters.Count];

	var ob = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	// Destroy(ob.GetComponent<CreateRegion>());
	// DestroyChildren(ob.transform);
	ob.transform.name = "gotchaaaa";

	
	// from https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
        int i = 0;
	MeshFilter mesh;
        while (i < meshFilters.Count)
        {
	    mesh =  meshFilters[i].GetComponent<MeshFilter>();
            combine[i].mesh = mesh.sharedMesh;
            combine[i].transform = mesh.transform.localToWorldMatrix;
            // meshFilters[i].SetActive(false);

	    meshFilters[i].transform.parent = ob.transform;
	    // Destroy(meshFilters[i]);
	    
            i++;
        }
	
        ob.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        ob.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
	ob.GetComponent<MeshRenderer>().enabled = true;
        ob.transform.gameObject.SetActive(true);
	ob.transform.parent = baseObject;
	PrefabUtility.SaveAsPrefabAsset(baseObject.gameObject, path);
	EditorApplication.isPlaying = false;
# endif
    }
}
