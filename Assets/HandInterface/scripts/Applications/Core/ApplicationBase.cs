using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;

namespace HPUI.Application.Core
{
    public class ApplicationBase : MonoBehaviour
    {
	public bool usesDeformableSurface=false;

        protected List<ButtonController> buttonsToRegister = new List<ButtonController>();
	
	public void Activate()
	{
	    gameObject.SetActive(true);
	    OnActivate();
	    InteractionManger.instance.getButtons();
            Debug.Log(buttonsToRegister.Count);
            foreach (var btn in buttonsToRegister)
            {
                // TODO: There seems to be an issue with the buttons that are invisible causing an issue in the button controller
                InteractionManger.instance.RegisterBtn(btn);
            }
	}

	public void Deacivate()
	{
	    OnDeactivate();
	    gameObject.SetActive(false);
	}

	protected virtual void OnDeactivate()
	{}

	protected virtual void OnActivate()
	{}
    }
}
