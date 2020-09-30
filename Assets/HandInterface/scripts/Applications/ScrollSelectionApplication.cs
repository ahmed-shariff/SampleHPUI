using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Application.Core;

namespace HPUI.Application.Sample.CarView
{
    public class ScrollSelectionApplication : ApplicationBase
    {
	public CarManager manager;
	public Transform p1;
	public Transform p2;
	public Transform p3;

	public ButtonController nextButton;
	public ButtonController previousButton;

	CarContainer c1;
	CarContainer c2;
	CarContainer c3;

	int currentIndex = 0;

	void Start()
	{
	    nextButton.contactAction.AddListener(next);
	    previousButton.contactAction.AddListener(prev);
	}
	
	protected override void OnActivate()
	{
	    setupViews();
	}

	protected override void OnDeactivate()
	{
	    resetCar(c1);
	    resetCar(c2);
	    resetCar(c3);
	}

	void setupViews()
	{
	    OnDeactivate();
	    if (currentIndex > 0)
		c1 = manager.Cars[currentIndex - 1];
	    else 
		c1 = null;
	    
	    c2 = manager.Cars[currentIndex];
	    manager.setCurrentCar(currentIndex);

	    if (currentIndex < manager.Cars.Count - 1)
		c3 = manager.Cars[currentIndex + 1];
	    else
		c3 = null;

	    setCarView(c1, p1);
	    setCarView(c2, p2);
	    setCarView(c3, p3);
	}

	void next(ButtonController btn)
	{
	    if (currentIndex < manager.Cars.Count - 1)
		currentIndex++;
	    setupViews();
	}

	void prev(ButtonController btn)
	{
	    if (currentIndex > 0)
		currentIndex--;
	    setupViews();
	}

	void setCarView(CarContainer c, Transform pos, float scaleFactor = 0.8f)
	{
	    if (c != null)
	    {
		c.gameObject.SetActive(true);
		c.setParent(pos);
		c.setScale(0.012f * scaleFactor);
	    }
	}

	void resetCar(CarContainer c, bool setActive = false)
	{
	    if (c != null)
	    {
		c.resetParent();
		c.resetScale();
		c.gameObject.SetActive(setActive);
	    }
	}
    }
}
