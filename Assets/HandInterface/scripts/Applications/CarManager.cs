using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HPUI.Application.Sample.CarView
{
    [DefaultExecutionOrder(-110)]
    public class CarManager : MonoBehaviour
    {
	public List<CarContainer> Cars = new List<CarContainer>();
	public static CarContainer currentCar {get; private set;}
	public bool configured {get; private set;} = false;

	void Start()
	{
	    foreach(var car in Cars)
		car.gameObject.SetActive(false);
	    currentCar = Cars[0];
	    currentCar.gameObject.SetActive(true);
	    configured = true;
	}

	public void setCurrentCar(int idx)
	{
	    currentCar = Cars[idx];
	}
    }
}
