using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;

namespace HPUI.Application.Core
{
    public class ApplicationManager : ApplicationBase
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
