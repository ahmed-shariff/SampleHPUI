using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLinker2 : MonoBehaviour
{
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parent.position;
        this.transform.rotation = Quaternion.Euler(parent.rotation.x, parent.rotation.y, parent.rotation.z + 90);
    }
}
