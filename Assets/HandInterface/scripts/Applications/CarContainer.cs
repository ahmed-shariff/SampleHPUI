using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Utils;

namespace HPUI.Application.Sample.CarView
{
    public class CarContainer : MonoBehaviour
    {
	public Material material;
	public Transform carObject;

	public Range scaleXRange;
	public Range scaleYRange;
	public Range scaleZRange;
	
	private Transform defaulParent;
	private Vector3 defaultScale;
	private Vector3 defaultPosition;
	private Quaternion defaultRotation;

	void Start()
	{
	    scaleXRange.normalizeScaleOn(carObject.localScale.x);
	    scaleYRange.normalizeScaleOn(carObject.localScale.y);
	    scaleZRange.normalizeScaleOn(carObject.localScale.z);
	}

	public void setParent(Transform parent)
	{
	    defaultPosition = carObject.localPosition;
	    defaultRotation = carObject.localRotation;
	    defaulParent = carObject.parent;
	    carObject.parent = parent;
	    carObject.localPosition = Vector3.zero;
	    carObject.localRotation = Quaternion.identity;
	}

	public void resetParent()
	{
	    carObject.parent = defaulParent;
	    carObject.localPosition = defaultPosition;
	    carObject.localRotation = defaultRotation;
	}

	public void setScale(float scaleFactor)
	{
	    defaultScale = carObject.localScale;
	    carObject.localScale = carObject.localScale * scaleFactor;
	}

	public Vector3 getScale()
	{
	    return carObject.localScale;
	}

	public void resetScale()
	{
	    carObject.localScale = defaultScale;
	}

	public void setScaleX(float scaleFactor)
	{
	    var scale = carObject.localScale;
	    scale.x = scaleXRange.getScaledValue(scaleFactor);
	    carObject.localScale = scale;
	}

	public void setScaleY(float scaleFactor)
	{
	    var scale = carObject.localScale;
	    scale.y = scaleYRange.getScaledValue(scaleFactor);
	    carObject.localScale = scale;
	}

	public void setScaleZ(float scaleFactor)
	{
	    var scale = carObject.localScale;
	    scale.z = scaleZRange.getScaledValue(scaleFactor);
	    carObject.localScale = scale;
	}
    }
}
