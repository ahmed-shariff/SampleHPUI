using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using HPUI.Application.Core;
using HPUI.Utils;
using RayCursor;

namespace HPUI.Application.Sample.InteriorDesign
{
    public class TransformObject : ApplicationBase
    {
	Color color;
	
	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
        public ObjectManager manager;

        public RayCursor.RayCursor rayCursor;

	public Range highlightXRange;
	public Range highlightYRange;

	public Color defaultColor;
	public Color highlightColor;

	public ButtonController xSelector;
	public ButtonController ySelector;
	public ButtonController zSelector;

        public ButtonController selectionBtn;
        public ButtonController selectionDoneBtn;
	
	List<ButtonController> highlightButtons;

        int x, y;
        
	Material material;
	Color materialColor;
	bool updatedScreen = false;

	float xLevel = 0.5f;
	float yLevel = 0.5f;	
	float zLevel = 0.5f;

	enum selection
	{
	    x,y,z
	}

	float maxX, maxY, minX, minY;

	selection currentSelection = selection.x;

	void Start()
	{
	    // xSelector.contactAction.AddListener(setX);
	    // ySelector.contactAction.AddListener(setY);
	    // zSelector.contactAction.AddListener(setZ);
            
            selectionBtn.contactAction.AddListener(selectionBtnEvent);
            selectionDoneBtn.contactAction.AddListener(selectionDoneBtnEvent);
        }
	
	void setup()
	{
	}

	protected override void OnActivate()
	{
	    //deformableSurfaceDisplayManager.inUse = true;
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

	// Update is called once per frame
	void Update()
	{
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
