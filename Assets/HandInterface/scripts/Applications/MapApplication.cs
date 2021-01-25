using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Application.Core;
using HPUI.Core.DeformableSurfaceDisplay;

namespace HPUI.Application.Sample
{
    public class MapApplication : ApplicationBase
    {
	public DeformableSurfaceDisplayManager btnMapperStatic;
	public PlaneMeshGenerator generatePlaneMesh;
	public Texture2D mainTexture;

	public SpriteRenderer spriteRenderer;
	public Sprite[] sprites = new Sprite[2];

	protected override void OnActivate()
	{
	    btnMapperStatic.inUse = true;
	    spriteRenderer.gameObject.SetActive(true);
	    generatePlaneMesh.GetComponent<MeshRenderer>().material.mainTexture = mainTexture;
	}

	protected override void OnDeactivate()
	{
	    btnMapperStatic.inUse = false;
	    spriteRenderer.gameObject.SetActive(false);
	}
	
	// Start is called before the first frame update
	void Start()
	{
	
	}

	// Update is called once per frame
	void Update()
	{
	    if (btnMapperStatic.currentCoord.x == 0 && btnMapperStatic.currentCoord.y == 0)
	    {
		spriteRenderer.gameObject.SetActive(false);
	    }
	    else
	    {
		spriteRenderer.gameObject.SetActive(true);
	    
		if ((btnMapperStatic.currentCoord.x / btnMapperStatic.currentCoord.maxX) > 0.7)
		    spriteRenderer.sprite = sprites[0];
		else
		    spriteRenderer.sprite = sprites[1];
	    }
	}
    }
}
