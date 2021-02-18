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
	Color color;
	
        public ObjectManager manager;

        public RayCursor.RayCursor rayCursor;

        public ButtonController selectionBtn;
        public ButtonController selectionDoneBtn;
	
	void Start()
	{
	    // xSelector.contactAction.AddListener(setX);
	    // ySelector.contactAction.AddListener(setY);
	    // zSelector.contactAction.AddListener(setZ);
            
            selectionBtn.contactAction.AddListener(selectionBtnEvent);
            selectionDoneBtn.contactAction.AddListener(selectionDoneBtnEvent);
        }
	
	protected override void OnActivate()
	{
	    selectionBtn.Show();
            selectionDoneBtn.Show();
            rayCursor.gameObject.SetActive(false);

            if (!buttonsToRegister.Contains(selectionDoneBtn))
                buttonsToRegister.Add(selectionDoneBtn);
	}

	protected override void OnDeactivate()
	{
            rayCursor.gameObject.SetActive(false);
	}
        
        void selectionBtnEvent(ButtonController btn=null)
        {
            // selectionBtn.Hide();
            // selectionDoneBtn.Show();
            rayCursor.gameObject.SetActive(true);
        }

        void selectionDoneBtnEvent(ButtonController btn=null)
        {
            // selectionBtn.Show();
            // selectionDoneBtn.Hide();
            rayCursor.PressButton();
            rayCursor.gameObject.SetActive(false);
        }
    }
}
