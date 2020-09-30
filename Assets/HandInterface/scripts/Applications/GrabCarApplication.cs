using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Application.Core;

namespace HPUI.Application.Sample.CarView
{
    public class GrabCarApplication : ApplicationBase
    {
	public Transform grabPosition;

	protected override void OnActivate()
	{
	    CarManager.currentCar.setScale(0.025f);
	    CarManager.currentCar.setParent(grabPosition);
	}

	protected override void OnDeactivate()
	{
	    CarManager.currentCar.resetParent();
	    CarManager.currentCar.resetScale();
	}
    }
}
