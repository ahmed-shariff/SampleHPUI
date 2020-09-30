using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPUI.Utils
{
    public class CameraFollower : MonoBehaviour
    {

	public Transform base1;
	public Transform base2;
	public Transform base3;
	public Transform base4;

	// Start is called before the first frame update
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
	    transform.position = base1.position;
	    Vector3 forward = base1.position - base2.position;
	    if (forward != Vector3.zero)
		transform.rotation = Quaternion.LookRotation(forward, Vector3.Cross(base3.position - base4.position, forward));
	}
    }
}
