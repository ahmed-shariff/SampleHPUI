using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HPUI.Application.Sample.CarView
{
    public class CarManager : MonoBehaviour
    {
	public static CarContainer currentCar;

	void Start()
	{
	    currentCar = GetComponentInChildren<CarContainer>();
	}
    }
}
