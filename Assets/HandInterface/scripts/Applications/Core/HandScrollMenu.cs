using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;

namespace HPUI.Application.Core
{
    public class HandScrollMenu : ApplicationBase
    {
	public ObjectManager manager;
	public Transform[] positions = new Transform[5];

        public Transform defaultObject;

	public ButtonController nextButton;
	public ButtonController previousButton;

        public Transform spawnPosition;

	Transform[] displayedObjects = new Transform[5];

        Transform[] tempObjects;
        
	int currentIndex = 0;

	void Start()
	{
	    nextButton.contactAction.AddListener(next);
	    previousButton.contactAction.AddListener(prev);
	}
	
	protected override void OnActivate()
	{
            tempObjects = new Transform[manager.Objects.Count];
	    setupViews();
	}

	protected override void OnDeactivate()
	{
            foreach(Transform obj in tempObjects)
            {
                if (obj)
                    Destroy(obj.gameObject);
            }

            OnSelectionExit();
	}

        public virtual void OnSelectionExit()
        {
            
        }

	void setupViews()
	{
            foreach(Transform obj in displayedObjects)
            {
                resetObject(obj);
            }

            for (var i = 0; i < 5 ; i++) // positions.Length??
            {
                var _i = currentIndex - 2 + i;
                Transform _obj;
                
                if (_i < 0 || _i >= manager.Objects.Count)
                {    
                    displayedObjects[i] = null;
                    continue;
                }
                else
                {
                    if (tempObjects[_i] == null)
                    {
                        _obj = Object.Instantiate(manager.Objects[_i].gameObject).transform;
                        _obj.localScale = _obj.localScale * 0.03f;
                        tempObjects[_i] = _obj;
                    }
                    else
                    {
                        _obj = tempObjects[_i];
                    }
                    displayedObjects[i] = _obj;
                }

                if (i == 2)
                {
                    manager.setCurrentObj(currentIndex);
                    setObjectView(_obj, positions[i], 1);
                }
                else
                {
                    setObjectView(_obj, positions[i]);
                }
            }
	}

	void next(ButtonController btn)
	{
	    if (currentIndex < manager.Objects.Count - 1)
		currentIndex++;
	    setupViews();
	}

	void prev(ButtonController btn)
	{
	    if (currentIndex > 0)
		currentIndex--;
	    setupViews();
	}

	void setObjectView(Transform c, Transform pos, float scaleFactor = 0.8f)
	{
	    if (c != null)
	    {
		c.gameObject.SetActive(true);
                c.parent = pos;
                c.position = pos.position;
                c.localRotation = Quaternion.identity;
	    }
	}

	void resetObject(Transform c, bool setActive = false)
	{
	    if (c != null)
	    {
                c.gameObject.SetActive(setActive);
	    }
	}

	void Update()
	{
	    // p1.Rotate(0, 0, 0.54f, Space.Self);
	    // p2.Rotate(0, 0, 0.47f, Space.Self);
	    // p3.Rotate(0, 0, 0.5f, Space.Self);
            foreach (var obj in displayedObjects)
            {
                if (obj)
                    obj.forward = Vector3.forward;
            }
	}
    }
}
