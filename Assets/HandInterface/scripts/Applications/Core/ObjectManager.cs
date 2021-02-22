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

        public bool hideObjectsOnInintialization = true;
        
        public GameObject objectSelectionColliderPrefab;

	void Start()
	{
            initialize();
        }

        public void initialize()
        {
            if (autoPopulate)
            {
                foreach (Transform child in transform)
                {
                    Objects.Add(child);
                    var s = child.GetComponentInChildren<Selectable>();
                    if (s)
                    {
                        s.OnSelect += OnSelect;
                    }
                    else if (objectSelectionColliderPrefab != null)
                    {
                        Vector3 min = new Vector3(100, 100, 100);
                        Vector3 max = new Vector3(-100, -100, -100);
                        var selfMeshRenderer = GetComponent<MeshRenderer>();
                        if (selfMeshRenderer != null)
                        {    
                            min = setMinPoints(selfMeshRenderer, min);
                            max = setMaxPoints(selfMeshRenderer, max);
                        }
                        foreach (var mr in child.GetComponentsInChildren<MeshRenderer>())
                        {
                            min = setMinPoints(mr, min);
                            max = setMaxPoints(mr, max);
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

            if (hideObjectsOnInintialization)
            {    
                foreach(var obj in Objects)
                    obj.gameObject.SetActive(false);
            }

            currentObject = Objects[0];
	    configured = true;
	}

        Vector3 setMinPoints(MeshRenderer mr, Vector3 min)
        {
            if (min.x > mr.bounds.min.x)
                min.x = mr.bounds.min.x;
            if (min.y > mr.bounds.min.y)
                min.y = mr.bounds.min.y;
            if (min.z > mr.bounds.min.z)
                min.z = mr.bounds.min.z;
            return min;
        }

        Vector3 setMaxPoints(MeshRenderer mr, Vector3 max)
        {
            if (max.x < mr.bounds.max.x)
                max.x = mr.bounds.max.x;
            if (max.y < mr.bounds.max.y)
                max.y = mr.bounds.max.y;
            if (max.z < mr.bounds.max.z)
                max.z = mr.bounds.max.z;
            return max;
        }

	public void setCurrentObj(int idx)
	{
	    currentObject = Objects[idx];
	}

        public void setCurrentObj(Transform obj)
	{
	    currentObject = obj;
	}

        void OnSelect(Selectable selectable)
        {
            currentObject = selectable.transform.parent;
        }

        public GameObject ReplicateCurrentObject()
        {
            var ob = UnityEngine.Object.Instantiate(currentObject.gameObject);
            ob.GetComponentInChildren<Selectable>().OnSelect += OnSelect;
            return ob;
        }
    }
}
