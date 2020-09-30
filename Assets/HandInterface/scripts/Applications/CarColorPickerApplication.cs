using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Application.Core;

namespace HPUI.Application.Sample.CarView
{
    public class CarColorPickerApplication : ApplicationBase
    {
	Color color;
	
	public BtnMapperStatic btnMapperStatic;
	public GeneratePlaneMesh generatePlaneMesh;
	public Texture2D mainTexture;

	public SpriteRenderer spriteRenderer;
	public Sprite[] sprites = new Sprite[2];

	protected override void OnActivate()
	{
	    btnMapperStatic.inUse = true;
	    spriteRenderer.gameObject.SetActive(true);
	    generatePlaneMesh.GetComponent<MeshRenderer>().material.mainTexture = mainTexture;

	    var materialColor = generatePlaneMesh.GetComponent<MeshRenderer>().material.color;
	    materialColor.a = 1;
	    generatePlaneMesh.GetComponent<MeshRenderer>().material.color = materialColor;
	}

	protected override void OnDeactivate()
	{
	    btnMapperStatic.inUse = false;
	    spriteRenderer.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
	    // TODO: replace this with the listner for the btns
	    if (btnMapperStatic.generatedBtns)
	    {
		color = mainTexture.GetPixel(Mathf.RoundToInt((btnMapperStatic.currentCoord.x / btnMapperStatic.currentCoord.maxX) * mainTexture.width), Mathf.RoundToInt((btnMapperStatic.currentCoord.y / btnMapperStatic.currentCoord.maxY) * mainTexture.height));
		if (spriteRenderer)
		    spriteRenderer.color = color;
		CarManager.currentCar.material.color = color;
	    }
	}
    }
}
