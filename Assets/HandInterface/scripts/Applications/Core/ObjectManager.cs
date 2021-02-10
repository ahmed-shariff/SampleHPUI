using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HPUI.Application.Core
{
    [DefaultExecutionOrder(-110)]
    public class ObjectManager : MonoBehaviour
    {
	public List<Transform> Objects = new List<Transform>();
	public Transform currentObject {get; private set;}
	public bool configured {get; private set;} = false;

        public bool autoPopulate = false;

	void Start()
	{
            if (autoPopulate)
            {
                foreach (Transform child in transform)
                {
                    Objects.Add(child);
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
    }
}
