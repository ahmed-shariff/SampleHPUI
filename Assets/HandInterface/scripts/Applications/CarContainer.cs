using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Start()
	{
	    defaulParent = carObject.transform.parent;

	    scaleXRange.normalizeScaleOn(carObject.localScale.x);
	    scaleYRange.normalizeScaleOn(carObject.localScale.y);
	    scaleZRange.normalizeScaleOn(carObject.localScale.z);
	}

	public void setParent(Transform parent)
	{
	    carObject.parent = parent;
	    carObject.position = Vector3.zero;
	}

	public void resetParent()
	{
	    carObject.parent = defaulParent;
	    carObject.position = Vector3.zero;
	}

	public void setScale(float scaleFactor)
	{
	    defaultScale = carObject.localScale;
	    carObject.localScale = carObject.localScale * scaleFactor;
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

    [Serializable]
    public class Range
    {
	public float min = 0.8f;
	public float max = 1.2f;

	public float getScaledValue(float scaleFactor)
	{
	    return (max - min) *  scaleFactor + min;
	}

	public void normalizeScaleOn(float normalizationValue)
	{
	    var mid = (min + max) / 2;
	    var halfRange = (max - min) / 2;
	    var normalizedHalfRange = (normalizationValue / mid) * halfRange;
	    min = mid - normalizedHalfRange;
	    max = mid + normalizedHalfRange;
	}
    }
}
