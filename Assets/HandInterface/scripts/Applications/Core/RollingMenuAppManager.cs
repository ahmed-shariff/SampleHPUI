using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;

namespace HPUI.Application.Core
{
    public class RollingMenuAppManager : ApplicationBase
    {
	public ButtonController baseButton;
	public ButtonController nextButton;
	public ButtonController previousButton;

	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
	
	public Transform menuDisplay;

	public ApplicationBase[] applications = new ApplicationBase[1];
	
	float currentDisplayAngle = 144;
	float currentAngle = 144;
	//TODO: make this be dynamically calculated?
	float angleIncrement = 5;
	// Vector3 currentTarget = Vector3.forward;

	bool showMenu = false;

	int currentAppIndex = 0;
	
	// Start is called before the first frame update
	void Start()
	{
	    // foreach(var app in applications)
	    // {
	    // 	app.Deacivate();
	    // }

            // For testing
	    // applications[0].Activate();
            // OnDeactivate();
            // *******************************************
            
	    baseButton.contactAction.AddListener(SwitchApp);
	    nextButton.contactAction.AddListener(incrementAppIndex);
	    previousButton.contactAction.AddListener(decrementAppIndex);
	    menuDisplay.localRotation = Quaternion.Euler(0, 144, 0);
	    // currentTarget = menuDisplay.forward;
	}

	void incrementAppIndex(ButtonController btn)
	{
	    currentAppIndex = (++currentAppIndex % applications.Length + applications.Length) % applications.Length;
	    currentDisplayAngle += 90;
	    // currentTarget = Quaternion.Euler(0, 72, 0) * currentTarget;
	}

	void decrementAppIndex(ButtonController btn)
	{
	    currentAppIndex = (--currentAppIndex % applications.Length + applications.Length) % applications.Length;
	    currentDisplayAngle -= 90;
	    // currentTarget = Quaternion.Euler(0, -72, 0) * currentTarget;
	}
	
	void SwitchApp(ButtonController btn)
	{
	    if (showMenu)
	    {
		applications[currentAppIndex].Deactivate();
		// deformableSurfaceDisplayManager.inUse = false;
		// deformableMesh.gameObject.SetActive(false);
		//currentAppIndex = ++currentAppIndex % applications.Length;
		ShowMenu();
	    }
	    else
	    {
		HideMenu();
		applications[currentAppIndex].Activate();
		Debug.Log("Swicthing application  " + applications[currentAppIndex].transform.name);
	    }
	    showMenu = !showMenu;
	}

	void ShowMenu()
	{
	    nextButton.gameObject.SetActive(true);
	    previousButton.gameObject.SetActive(true);
	    InteractionManger.instance.GetButtons();
	    menuDisplay.transform.parent.gameObject.SetActive(true);
	}

	void HideMenu()
	{
	    nextButton.gameObject.SetActive(false);
	    previousButton.gameObject.SetActive(false);
	    menuDisplay.transform.parent.gameObject.SetActive(false);
	}
	
	protected virtual void OnActivate()
	{
	    showMenu = true;
	    ShowMenu();
	}

	protected virtual void OnDeactivate()
	{
	    HideMenu();
	    showMenu = true;
	    applications[currentAppIndex].Deactivate();
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
