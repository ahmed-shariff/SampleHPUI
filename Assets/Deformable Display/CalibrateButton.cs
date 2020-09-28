using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateButton : MonoBehaviour
{

    public BtnMapperStatic btnMapperStatic;
    public HandCoordinateGetter handCoordinateGetter;

    private void OnTriggerEnter(Collider other)
    {
       	OnClick();
    }

    public void OnClick()
    {
	if (!handCoordinateGetter.isCalibrated)
	{
	    handCoordinateGetter.Calibrate();
	    btnMapperStatic.generateBtns();
	}
    }
}
