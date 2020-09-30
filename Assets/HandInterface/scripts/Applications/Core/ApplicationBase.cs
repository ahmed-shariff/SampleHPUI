using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;

namespace HPUI.Application.Core
{
    public class ApplicationBase : MonoBehaviour
    {
	public bool usesDeformableSurface=false;
	
	public void Activate()
	{
	    gameObject.SetActive(true);
	    OnActivate();
	    InteractionManger.instance.getButtons();
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
