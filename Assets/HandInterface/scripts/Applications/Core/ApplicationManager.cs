using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;

namespace HPUI.Application.Core
{
    public class ApplicationManager : ApplicationBase
    {
	public ApplicationBase[] applications = new ApplicationBase[1];
	public ButtonController baseButton;
	public ButtonController nextButton;
	public ButtonController previousButton;

	public Transform menuDisplay;

	float currentDisplayAngle = 144;
	float currentAngle = 144;
	float angleIncrement = 5;
	// Vector3 currentTarget = Vector3.forward;

	bool showMenu = true;

	int currentAppIndex = 0;
	
	// Start is called before the first frame update
	void Start()
	{
	    foreach(var app in applications)
	    {
		app.Deacivate();
	    }
	    baseButton.contactAction.AddListener(SwitchApp);
	    nextButton.contactAction.AddListener(incrementAppIndex);
	    previousButton.contactAction.AddListener(decrementAppIndex);
	    menuDisplay.localRotation = Quaternion.Euler(0, 144, 0);
	    // currentTarget = menuDisplay.forward;
	}

	void incrementAppIndex(ButtonController btn)
	{
	    currentAppIndex = (++currentAppIndex % applications.Length + applications.Length) % applications.Length;
	    currentDisplayAngle += 72;
	    // currentTarget = Quaternion.Euler(0, 72, 0) * currentTarget;
	}

	void decrementAppIndex(ButtonController btn)
	{
	    currentAppIndex = (--currentAppIndex % applications.Length + applications.Length) % applications.Length;
	    currentDisplayAngle -= 72;
	    // currentTarget = Quaternion.Euler(0, -72, 0) * currentTarget;
	}
	
	void SwitchApp(ButtonController btn)
	{
	    if (showMenu)
	    {
		applications[currentAppIndex].Deacivate();
		//currentAppIndex = ++currentAppIndex % applications.Length;
		OnActivate();
	    }
	    else
	    {
		OnDeactivate();
		applications[currentAppIndex].Activate();
		Debug.Log("Swicthing application  " + applications[currentAppIndex].transform.name);
	    }
	    showMenu = !showMenu;
	}

	protected virtual void OnActivate()
	{
	    nextButton.gameObject.SetActive(true);
	    previousButton.gameObject.SetActive(true);
	    InteractionManger.instance.getButtons();
	    menuDisplay.transform.parent.gameObject.SetActive(true);
	}

	protected virtual void OnDeactivate()
	{
	    nextButton.gameObject.SetActive(false);
	    previousButton.gameObject.SetActive(false);
	    menuDisplay.transform.parent.gameObject.SetActive(false);
	}

	void Update()
	{
	    if (Mathf.Abs(currentAngle - currentDisplayAngle) > angleIncrement)
	    {
		menuDisplay.localRotation = Quaternion.Euler(0, currentAngle, 0);
		if (currentAngle < currentDisplayAngle)
		    currentAngle += angleIncrement;
		else
		    currentAngle -= angleIncrement;
	    }
	}
    }
}
