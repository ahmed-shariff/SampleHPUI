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
        public ButtonController selectionDoneBtn;
	
	void Start()
	{
	    // xSelector.contactAction.AddListener(setX);
	    // ySelector.contactAction.AddListener(setY);
	    // zSelector.contactAction.AddListener(setZ);

            if (selectionBtn)
                selectionBtn.contactAction.AddListener(selectionBtnEvent);
            if (selectionDoneBtn)
                selectionDoneBtn.contactAction.AddListener(selectionDoneBtnEvent);
        }
	
	protected override void OnActivate()
	{
            if (selectionBtn)
                selectionBtn.Show();
            if (selectionDoneBtn)
                selectionDoneBtn.Hide();
            rayCursor.gameObject.SetActive(false);

            if (selectionDoneBtn != null && !buttonsToRegister.Contains(selectionDoneBtn))
                buttonsToRegister.Add(selectionDoneBtn);
	}

	protected override void OnDeactivate()
	{
            rayCursor.gameObject.SetActive(false);
	}
        
        protected virtual void selectionBtnEvent(ButtonController btn=null)
        {
	    InteractionManger.instance.RegisterBtn(selectionDoneBtn);
            selectionBtn.Hide();
            selectionDoneBtn.Show();
            rayCursor.gameObject.SetActive(true);
        }

        protected virtual void selectionDoneBtnEvent(ButtonController btn=null)
        {
            selectionBtn.Show();
            selectionDoneBtn.Hide();
            rayCursor.PressButton();
            rayCursor.gameObject.SetActive(false);
        }
    }
}
