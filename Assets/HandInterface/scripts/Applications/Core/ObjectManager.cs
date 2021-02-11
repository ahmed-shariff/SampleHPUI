using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayCursor;

namespace HPUI.Application.Core
{
    [DefaultExecutionOrder(-110)]
    public class ObjectManager : MonoBehaviour
    {
	public List<Transform> Objects = new List<Transform>();
	public Transform currentObject {get; private set;}
	public bool configured {get; private set;} = false;

        public bool autoPopulate = false;

        public GameObject objectSelectionColliderPrefab;

	void Start()
	{
            if (autoPopulate)
            {
                int id = 0;
                foreach (Transform child in transform)
                {
                    Objects.Add(child);
                    var s = child.GetComponentInChildren<Selectable>();
                    if (s)
                    {
                        s.OnSelect += OnSelect;
                    }
                    else
                    {
                        Vector3 min = new Vector3(100, 100, 100);
                        Vector3 max = new Vector3(-100, -100, -100);
                        foreach (var mr in child.GetComponentsInChildren<MeshRenderer>())
                        {
                            Debug.Log(child.name +" :: " + mr.bounds.min);
                            if (min.x > mr.bounds.min.x)
                                min.x = mr.bounds.min.x;
                            if (min.y > mr.bounds.min.y)
                                min.y = mr.bounds.min.y;
                            if (min.z > mr.bounds.min.z)
                                min.z = mr.bounds.min.z;

                            if (max.x < mr.bounds.max.x)
                                max.x = mr.bounds.max.x;
                            if (max.y < mr.bounds.max.y)
                                max.y = mr.bounds.max.y;
                            if (max.z < mr.bounds.max.z)
                                max.z = mr.bounds.max.z;
                        }
                        var diff = max-min;
                        var ob = Instantiate(objectSelectionColliderPrefab, diff/2 + min, Quaternion.identity);
                        ob.transform.localScale = diff * 0.01f + diff;
                        ob.transform.parent = child;

                        s = child.GetComponentInChildren<Selectable>();
                        s.OnSelect += OnSelect;
                    }
                }
            }
            
	    foreach(var obj in Objects)
		obj.gameObject.SetActive(false);
	    currentObject = Objects[0];
	    configured = true;
	}

	public void setCurrentObj(int idx)
	{
	    currentObject = Objects[idx];
	}   

        void OnSelect(Selectable selectable)
        {
            currentObject = selectable.transform.parent;
        }
    }
}
