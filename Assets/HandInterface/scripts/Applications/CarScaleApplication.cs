using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Application.Core;
using HPUI.Utils;

namespace HPUI.Application.Sample.CarView
{
    public class CarScaleApplication : ApplicationBase
    {
	Color color;
	
	public BtnMapperStatic btnMapperStatic;
	public DeformableMesh deformableMesh;

	public Range highlightXRange;
	public Range highlightYRange;

	public Color defaultColor;
	public Color highlightColor;

	public ButtonController xSelector;
	public ButtonController ySelector;
	public ButtonController zSelector;
	
	List<ButtonController> highlightButtons;

	Coord coord = new Coord();

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
	    maxX = GeneratePlaneMesh.x_divisions * highlightXRange.max;
	    maxY = GeneratePlaneMesh.y_divisions * highlightYRange.max;
	    
	    minX = GeneratePlaneMesh.x_divisions * highlightXRange.min;
	    minY = GeneratePlaneMesh.y_divisions * highlightYRange.min;
		    
	    foreach(var btn in btnMapperStatic.buttonControllers)
	    {
		GeneratePlaneMesh.btnIdToxy(btn.id, out coord.x, out coord.y);
		if (coord.x >= minX && coord.y >= minY && coord.x < maxX && coord.y < maxY)
		{
		    highlightButtons.Add(btn);
		    // deformableMesh.skipIds.Add(btn.id);
		    btn.contactAction.AddListener(updateBar);
		}
		else
		{
		    btn.gameObject.SetActive(false);
		}
	    }
	    material = deformableMesh.GetComponent<MeshRenderer>().material;
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

	void updateBar(ButtonController btn)
	{
	    float currentThresh = 0.5f;
	    switch(currentSelection)
	    {
		case selection.x:
		    if (btn != null)
			xLevel = btnMapperStatic.currentCoord.y / (maxY - minY);
		    CarManager.currentCar.setScaleX(xLevel);
		    currentThresh = xLevel * (maxY - minY);
		    break;
		case selection.y:
		    if (btn != null)
			yLevel = btnMapperStatic.currentCoord.y / (maxY - minY);
		    CarManager.currentCar.setScaleY(yLevel);
		    currentThresh = yLevel * (maxY - minY);
		    break;
		case selection.z:
		    if (btn != null)
			zLevel = btnMapperStatic.currentCoord.y / (maxY - minY);
		    CarManager.currentCar.setScaleZ(zLevel);
		    currentThresh = zLevel * (maxY - minY);
		    break;
	    }
	    
	    try
	    {
		foreach(var otherBtn in highlightButtons)
		{
		    GeneratePlaneMesh.btnIdToxy(otherBtn.id, out coord.x, out coord.y);
		    if (coord.y >= currentThresh)
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
	    btnMapperStatic.inUse = true;
	}

	protected override void OnDeactivate()
	{
	    btnMapperStatic.inUse = false;
	    highlightButtons = null;
	    // deformableMesh.skipIds.Clear();
	    foreach(var btn in btnMapperStatic.buttonControllers)
	    {
		btn.gameObject.SetActive(true);
		btn.contactAction.RemoveListener(updateBar);
		btn.setSelectionDefault(false);
		btn.invokeDefault();
		materialColor.a = 1;
		material.color = materialColor;
	    }
	    if (material)
	    {
		materialColor.a = 0;
		material.color = materialColor;
	    }
	}

	// Update is called once per frame
	void Update()
	{
	    if (btnMapperStatic.generatedBtns)
	    {
		if (highlightButtons == null)
		{
		    setup();
		}
		if (!updatedScreen)
		{
		    updateBar(null);
		}
	    }
	}
    }
}
