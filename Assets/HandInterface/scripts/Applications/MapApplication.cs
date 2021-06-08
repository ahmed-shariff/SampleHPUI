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
	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
	public Texture2D mainTexture;

	public SpriteRenderer spriteRenderer;
	public Sprite[] sprites = new Sprite[2];

	protected override void OnActivate()
	{
	    deformableSurfaceDisplayManager.inUse = true;
	    // spriteRenderer.gameObject.SetActive(true);
	    deformableSurfaceDisplayManager.MeshRenderer.material.mainTexture = mainTexture;
	}

	protected override void OnDeactivate()
	{
	    deformableSurfaceDisplayManager.inUse = false;
	    // spriteRenderer.gameObject.SetActive(false);
	}
	
	// Start is called before the first frame update
	void Start()
	{
	
	}

	// Update is called once per frame
	void Update()
	{
	    if (deformableSurfaceDisplayManager.currentCoord.x == 0 && deformableSurfaceDisplayManager.currentCoord.y == 0)
	    {
		// spriteRenderer.gameObject.SetActive(false);
	    }
	    else
	    {
		// spriteRenderer.gameObject.SetActive(true);
	    
		// if ((deformableSurfaceDisplayManager.currentCoord.x / deformableSurfaceDisplayManager.currentCoord.maxX) > 0.7)
		//     spriteRenderer.sprite = sprites[0];
		// else
		//     spriteRenderer.sprite = sprites[1];
	    }
	}
    }
}
