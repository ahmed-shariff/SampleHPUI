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
	}

	void incrementAppIndex(ButtonController btn)
	{
	    currentAppIndex = (++currentAppIndex % applications.Length + applications.Length) % applications.Length;
	}

	void decrementAppIndex(ButtonController btn)
	{
	    currentAppIndex = (--currentAppIndex % applications.Length + applications.Length) % applications.Length;
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
	}

	protected virtual void OnDeactivate()
	{
	    nextButton.gameObject.SetActive(false);
	    previousButton.gameObject.SetActive(false);
	}
    }
}
