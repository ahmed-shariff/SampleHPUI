using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using System;

namespace HPUI.Application.Core
{
    public class FlatMenu : ApplicationBase
    {
	public ButtonController baseButton;

	public ApplicationControl[] applications = new ApplicationControl[1];

	private int currentAppIndex = -1;
	private bool showMenu = true;
	
	// Start is called before the first frame update
	void Start()
	{
	    baseButton.contactAction.AddListener(SwitchApp);

	    for(int i=0; i < applications.Length; i++)
	    {
		var idx = i;
	        applications[i].button.contactAction.AddListener((btn) => {currentAppIndex = idx; HideMenu();});
		if (applications[i].layer > 0)
		    Camera.main.cullingMask &= ~(1 << applications[i].layer);
	    }
	}
	
	void SwitchApp(ButtonController btn)
	{
	    if (currentAppIndex < 0)
		return;
	    
	    if (showMenu)
	    {
		ShowMenu();
	    }
	    else
	    {
	        HideMenu();
		Debug.Log("Swicthing application  " + applications[currentAppIndex].application.transform.name);
	    }
	    showMenu = !showMenu;
	}

	void ShowMenu()
	{
	    foreach(var app in applications)
	    {
		app.button.Show();
		if (app.layer > 0)
		{
		    Camera.main.cullingMask &= ~(1 << app.layer);
		}
	    }
	    InteractionManger.instance.getButtons();

	    applications[currentAppIndex].application.Deactivate();
	}

	void HideMenu()
	{
	    foreach(var app in applications)
	    {
		app.button.Hide();
		if (app.layer > 0)
		{
		    Camera.main.cullingMask |= 1 << app.layer;
		}
	    }
	    Debug.Log($"Trying to {currentAppIndex}");
	    applications[currentAppIndex].application.Activate();
	}

	[Serializable]
	public class ApplicationControl
	{
	    public ApplicationBase application;
	    public ButtonController button;
	    public int layer;
	}
    }
}
