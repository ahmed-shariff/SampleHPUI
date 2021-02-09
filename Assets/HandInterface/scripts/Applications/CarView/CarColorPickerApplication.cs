using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using HPUI.Application.Core;

namespace HPUI.Application.Sample.CarView
{
    public class CarColorPickerApplication : ApplicationBase
    {
	Color color;
	
	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
	public Texture2D mainTexture;

	public SpriteRenderer spriteRenderer;
	public Sprite[] sprites = new Sprite[2];

	protected override void OnActivate()
	{
	    deformableSurfaceDisplayManager.inUse = true;
	    spriteRenderer.gameObject.SetActive(true);
	    deformableSurfaceDisplayManager.MeshRenderer.material.mainTexture = mainTexture;

	    var materialColor = deformableSurfaceDisplayManager.MeshRenderer.material.color;
	    materialColor.a = 1;
	    deformableSurfaceDisplayManager.MeshRenderer.material.color = materialColor;
	}

	protected override void OnDeactivate()
	{
	    deformableSurfaceDisplayManager.inUse = false;
	    spriteRenderer.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
	    // TODO: replace this with the listner for the btns
	    if (deformableSurfaceDisplayManager.generatedBtns)
	    {
		color = mainTexture.GetPixel(Mathf.RoundToInt((deformableSurfaceDisplayManager.currentCoord.x / deformableSurfaceDisplayManager.currentCoord.maxX) * mainTexture.width), Mathf.RoundToInt((deformableSurfaceDisplayManager.currentCoord.y / deformableSurfaceDisplayManager.currentCoord.maxY) * mainTexture.height));
		if (spriteRenderer)
		    spriteRenderer.color = color;
		CarManager.currentCar.material.color = color;
	    }
	}
    }
}
