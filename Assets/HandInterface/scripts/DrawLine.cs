using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(130)]
public class DrawLine : MonoBehaviour
{
    public Transform root;
    public Transform target;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lr = transform.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        // Set some positions
        lr.SetPosition(0, root.position);
        lr.SetPosition(1, target.position);
        lr.SetWidth(0.001f, 0.001f);

        if (transform.childCount > 0)
        {
	        Vector3 _up = target.position - root.position;
            if (_up != Vector3.zero)
            {
                Transform t = transform.GetChild(0);
                t.localScale = root.lossyScale;
                t.position = root.position;
                t.rotation = Quaternion.LookRotation(root.forward, _up);
                t.localPosition += _up * 0.5f;
                Vector3 _localScale = t.localScale;
                _localScale.y = _up.magnitude/2;
                t.localScale = _localScale;
                //t.localScale *= t.up * _up.magnitude;
            }
        }
    }
}
