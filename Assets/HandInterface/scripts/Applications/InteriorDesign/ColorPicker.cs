﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using HPUI.Application.Core;
using HPUI.Utils;
using RayCursor;

namespace HPUI.Application.Sample.InteriorDesign
{
    public class ColorPicker : ObjectSelection
    {
	Color color;
	
	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
        public Texture2D mainTexture;
        
        public SpriteRenderer spriteRenderer;

	void Start()
	{
	    // xSelector.contactAction.AddListener(setX);
	    // ySelector.contactAction.AddListener(setY);
	    // zSelector.contactAction.AddListener(setZ);
            
            selectionBtn.contactAction.AddListener(selectionBtnEvent);
            selectionDoneBtn.contactAction.AddListener(selectionDoneBtnEvent);
        }
	
	protected override void OnActivate()
	{
            base.OnActivate();
	    deformableSurfaceDisplayManager.inUse = true;
            spriteRenderer.gameObject.SetActive(true);
	    deformableSurfaceDisplayManager.MeshRenderer.material.mainTexture = mainTexture;

	    var materialColor = deformableSurfaceDisplayManager.MeshRenderer.material.color;
	    materialColor.a = 1;
	    deformableSurfaceDisplayManager.MeshRenderer.material.color = materialColor;

	}

	protected override void OnDeactivate()
	{
            base.OnDeactivate();
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
                var m = manager.currentObject.GetComponentsInChildren<MeshRenderer>()[0].material;
		m.color = color;
	    }
	}

        void selectionBtnEvent(ButtonController btn=null)
        {
            // selectionBtn.Hide();
            // selectionDoneBtn.Show();
            deformableSurfaceDisplayManager.inUse = false;
            rayCursor.gameObject.SetActive(true);
        }

        void selectionDoneBtnEvent(ButtonController btn=null)
        {
            // selectionBtn.Show();
            // selectionDoneBtn.Hide();
            rayCursor.PressButton();
            deformableSurfaceDisplayManager.inUse = true;
            rayCursor.gameObject.SetActive(false);
        }
    }
}
