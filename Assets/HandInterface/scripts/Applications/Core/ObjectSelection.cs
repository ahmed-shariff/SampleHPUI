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
	public ButtonController cancelBtn;

        protected bool inSelection = false;
        // public ButtonController selectionDoneBtn;
	
	void Start()
	{
	    // xSelector.contactAction.AddListener(setX);
	    // ySelector.contactAction.AddListener(setY);
	    // zSelector.contactAction.AddListener(setZ);

            if (selectionBtn)
                selectionBtn.contactAction.AddListener(selectionBtnEvent);
	    if (cancelBtn)
	    {
		cancelBtn.contactAction.AddListener(cancelBtnEvent);
		cancelBtn.Hide();
	        LateRegsiterBtn(cancelBtn);
	    }
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
	        selectionStateClear();
	    }
	    else
	    {
		rayCursor.gameObject.SetActive(true);
		btn.setSelectionHighlight(false);
		btn.invokeDefault();
		if (cancelBtn)
		    cancelBtn.Show();
	    }
	    inSelection = !inSelection;
        }

	protected virtual void selectionStateClear()
	{
	    rayCursor.gameObject.SetActive(false);
	    if (selectionBtn)
	    {
		selectionBtn.setSelectionHighlight(true);
	        selectionBtn.invokeDefault();
	    }

	    if (cancelBtn)
		cancelBtn.Hide();
	}

	protected virtual void cancelBtnEvent(ButtonController btn)
	{
	    selectionStateClear();
	    inSelection = false;
	}
    }
}
