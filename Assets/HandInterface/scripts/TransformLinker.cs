using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLinker : MonoBehaviour
{
    public Transform parent;
    public Transform secondParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (secondParent)
        {
            Vector3 interDirection = secondParent.position - parent.position;
            if (interDirection != Vector3.zero)
            {
                this.transform.position = parent.position + (interDirection) * 0.5f;
                // this.transform.rotation = Quaternion.Slerp(parent.rotation, secondParent.rotation, 0.5f);
                this.transform.rotation = Quaternion.LookRotation((secondParent.forward - parent.forward)/2 + parent.forward, parent.up);
            }
        }
        else
        {
            this.transform.position = parent.position;
            this.transform.rotation = parent.rotation;
        }
    }
}
