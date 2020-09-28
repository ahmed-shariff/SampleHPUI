using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaPlayerApplication : MonoBehaviour
{
    public List<Texture> textureList;

    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
	material.mainTexture = textureList[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
