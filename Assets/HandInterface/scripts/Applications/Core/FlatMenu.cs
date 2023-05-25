using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	private bool showMenu = false;
	private List<ButtonController> btnsForShow;
	
	// Start is called before the first frame update
	void Start()
	{
	    baseButton.contactAction.AddListener(SwitchApp);

	    for(int i=0; i < applications.Length; i++)
	    {
		var idx = i;
	        applications[i].button.contactAction.AddListener((btn) => {
		    currentAppIndex = idx;
		    showMenu = false;
		    SwitchApp(btn);
		});
		if (applications[i].layer > 0)
		    Camera.main.cullingMask &= ~(1 << applications[i].layer);
	    }
	    var notForShowBtns = applications.Select(x => x.button).ToList();
	    btnsForShow = new List<ButtonController>();
	    notForShowBtns.Add(baseButton);
	    foreach(var btn in GetComponentsInChildren<ButtonController>())
	    {
		if (!notForShowBtns.Contains(btn))
		{
		    btnsForShow.Add(btn);
		}
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
	    }
	    showMenu = !showMenu;
	}

	void ShowMenu()
	{
	    applications[currentAppIndex].application.Deactivate();
	    foreach(var app in applications)
	    {
		app.button.Show();
		if (app.layer > 0)
		{
		    Camera.main.cullingMask &= ~(1 << app.layer);
		}
	    }

	    foreach(var btn in btnsForShow)
	    {
		btn.Show();
	    }

	    InteractionManger.instance.GetButtons();
	}

	void HideMenu()
	{
	    foreach(var app in applications)
	    {
		app.button.Hide();
	    }

	    foreach(var btn in btnsForShow)
	    {
		btn.Hide();
	    }

	    
	    if (applications[currentAppIndex].layer > 0)
	    {
		Camera.main.cullingMask |= 1 << applications[currentAppIndex].layer;
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
