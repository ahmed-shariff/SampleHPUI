using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using HPUI.Application.Core;
using HPUI.Utils;
using RayCursor;

namespace HPUI.Application.Core
{
    public class ObjectSelection : ApplicationBase
    {
	public ObjectManager manager;

        public RayCursor.RayCursor rayCursor;

        public ButtonController selectionBtn;

	private bool inSelection = false;
        // public ButtonController selectionDoneBtn;
	
	void Start()
	{
	    // xSelector.contactAction.AddListener(setX);
	    // ySelector.contactAction.AddListener(setY);
	    // zSelector.contactAction.AddListener(setZ);

            if (selectionBtn)
                selectionBtn.contactAction.AddListener(selectionBtnEvent);
        }
	
	protected override void OnActivate()
	{
            if (selectionBtn)
                selectionBtn.Show();
            rayCursor.gameObject.SetActive(false);
	}

	protected override void OnDeactivate()
	{
            rayCursor.gameObject.SetActive(false);
	}
        
        protected virtual void selectionBtnEvent(ButtonController btn=null)
        {
	    if (inSelection)
	    {
        	rayCursor.PressButton();
		rayCursor.gameObject.SetActive(false);
		btn.setSelectionHighlight(true);
		btn.invokeDefault();
	    }
	    else
	    {
		rayCursor.gameObject.SetActive(true);
		btn.setSelectionHighlight(false);
		btn.invokeDefault();
	    }
	    inSelection = !inSelection;
        }
    }
}
