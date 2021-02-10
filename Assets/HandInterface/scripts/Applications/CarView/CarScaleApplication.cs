using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using HPUI.Application.Core;
using HPUI.Utils;

namespace HPUI.Application.Sample.CarView
{
    public class CarScaleApplication : ApplicationBase
    {
	Color color;
	
	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;

	public Range highlightXRange;
	public Range highlightYRange;

	public Color defaultColor;
	public Color highlightColor;

	public ButtonController xSelector;
	public ButtonController ySelector;
	public ButtonController zSelector;
	
	List<ButtonController> highlightButtons;

        int x, y;
        
	Material material;
	Color materialColor;
	bool updatedScreen = false;

	float xLevel = 0.5f;
	float yLevel = 0.5f;	
	float zLevel = 0.5f;

	enum selection
	{
	    x,y,z
	}

	float maxX, maxY, minX, minY;

	selection currentSelection = selection.x;

	void Start()
	{
	    xSelector.contactAction.AddListener(setX);
	    ySelector.contactAction.AddListener(setY);
	    zSelector.contactAction.AddListener(setZ);
	}
	
	void setup()
	{
	    var scale = CarManager.currentCar.getScale();
	    xLevel = CarManager.currentCar.scaleXRange.getInverseScaledValue(scale.x);
	    yLevel = CarManager.currentCar.scaleYRange.getInverseScaledValue(scale.y);
	    zLevel = CarManager.currentCar.scaleZRange.getInverseScaledValue(scale.z);
	    highlightButtons = new List<ButtonController>();
	    maxX = deformableSurfaceDisplayManager.currentCoord.maxX * highlightXRange.max;
	    maxY = deformableSurfaceDisplayManager.currentCoord.maxY * highlightYRange.max;
	    
	    minX = deformableSurfaceDisplayManager.currentCoord.maxX * highlightXRange.min;
	    minY = deformableSurfaceDisplayManager.currentCoord.maxY * highlightYRange.min;
		    
	    foreach(var btn in deformableSurfaceDisplayManager.buttonControllers)
	    {
		deformableSurfaceDisplayManager.idToXY(btn.id, out x, out y);
		if (x >= minX && y >= minY && x < maxX && y < maxY)
		{
		    highlightButtons.Add(btn);
		    // deformableMesh.skipIds.Add(btn.id);
		    // btn.contactAction.AddListener(updateBar);
		}
		else
		{
		    btn.gameObject.SetActive(false);
		}
	    }
	    material = deformableSurfaceDisplayManager.MeshRenderer.material;
	    materialColor = material.color;
	    materialColor.a = 0;
	    material.color = materialColor;
	}

	void setX(ButtonController btn)
	{
	    currentSelection = selection.x;
	    xSelector.setSelectionDefault(true, defaultColor);
	    xSelector.invokeDefault();
	    
	    ySelector.setSelectionDefault(false);
	    ySelector.invokeDefault();
	    
	    zSelector.setSelectionDefault(false);
	    zSelector.invokeDefault();
	}

	void setY(ButtonController btn)
	{
	    currentSelection = selection.y;
	    ySelector.setSelectionDefault(true, defaultColor);
	    ySelector.invokeDefault();
	    
	    xSelector.setSelectionDefault(false);
	    xSelector.invokeDefault();
	    
	    zSelector.setSelectionDefault(false);
	    zSelector.invokeDefault();
	}

	void setZ(ButtonController btn)
	{
	    currentSelection = selection.z;
	    zSelector.setSelectionDefault(true, defaultColor);
	    zSelector.invokeDefault();
	    
	    ySelector.setSelectionDefault(false);
	    ySelector.invokeDefault();
	    
	    xSelector.setSelectionDefault(false);
	    xSelector.invokeDefault();
	}

	void updateBar(bool btn)
	{
	    float currentThresh = 0.5f;
	    switch(currentSelection)
	    {
		case selection.x:
		    if (btn)
			xLevel = deformableSurfaceDisplayManager.currentCoord.y / (maxY - minY);
		    CarManager.currentCar.setScaleX(xLevel);
		    currentThresh = xLevel * (maxY - minY);
		    break;
		case selection.y:
		    if (btn)
			yLevel = deformableSurfaceDisplayManager.currentCoord.y / (maxY - minY);
		    CarManager.currentCar.setScaleY(yLevel);
		    currentThresh = yLevel * (maxY - minY);
		    break;
		case selection.z:
		    if (btn)
			zLevel = deformableSurfaceDisplayManager.currentCoord.y / (maxY - minY);
		    CarManager.currentCar.setScaleZ(zLevel);
		    currentThresh = zLevel * (maxY - minY);
		    break;
	    }
	    
	    try
	    {
		foreach(var otherBtn in highlightButtons)
		{
		    deformableSurfaceDisplayManager.idToXY(otherBtn.id, out x, out y);
		    if (y >= currentThresh)
		    {
			otherBtn.setSelectionDefault(true, defaultColor);
		    }
		    else
		    {
			otherBtn.setSelectionDefault(true, highlightColor);
		    }
		    otherBtn.invokeDefault();
		}
		updatedScreen = true;
	    }
	    catch(NullReferenceException e)
	    {}
	}

	protected override void OnActivate()
	{
	    deformableSurfaceDisplayManager.inUse = true;
	}

	protected override void OnDeactivate()
	{
	    deformableSurfaceDisplayManager.inUse = false;
	    highlightButtons = null;
	    // deformableMesh.skipIds.Clear();
	    foreach(var btn in deformableSurfaceDisplayManager.buttonControllers)
	    {
		btn.gameObject.SetActive(true);
		// btn.contactAction.RemoveListener(updateBar);
		btn.setSelectionDefault(false);
		btn.invokeDefault();
	    }
	    materialColor.a = 1;
	    material.color = materialColor;
	    updatedScreen = false;
	}

	// Update is called once per frame
	void Update()
	{
	    if (deformableSurfaceDisplayManager.generatedBtns)
	    {
		if (highlightButtons == null)
		{
		    setup();
		}
		if (!updatedScreen)
		{
		    updateBar(false);
		}
		else
		{
		    updateBar(true);
		}
	    }
	}
    }
}
