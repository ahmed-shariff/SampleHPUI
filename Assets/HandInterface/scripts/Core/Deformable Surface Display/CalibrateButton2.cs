using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HPUI.Core.DeformableSurfaceDisplay;

namespace HPUI.Core
{
    public class CalibrateButton2 : MonoBehaviour
    {

	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
	public DeformationCoordinateManager deformationCoordinateManager;

	private void OnTriggerEnter(Collider other)
	{
	    OnClick();
	}

	public void OnClick()
	{
	    if (!deformationCoordinateManager.isCalibrated())
	    {
		deformationCoordinateManager.Calibrate();
		deformableSurfaceDisplayManager.generateBtns();
	    }
	}
    }
}
