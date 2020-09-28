using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerApplication : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material material;
    public BtnMapperStatic btnMapperStatic;

    Texture2D tex;
    GeneratePlaneMesh generatePlaneMesh;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        tex = (Texture2D) GetComponent<MeshRenderer>().material.mainTexture;
	generatePlaneMesh = GetComponent<GeneratePlaneMesh>();
    }

    // Update is called once per frame
    void Update()
    {
	if (btnMapperStatic.generatedBtns)
	{
	    color = tex.GetPixel(Mathf.RoundToInt((btnMapperStatic.currentCoord.x / btnMapperStatic.currentCoord.maxX) * tex.width), Mathf.RoundToInt((btnMapperStatic.currentCoord.y / btnMapperStatic.currentCoord.maxY) * tex.height));
	    if (spriteRenderer)
		spriteRenderer.color = color;
	    if (material)
		material.color = color;
	}
    }
}
