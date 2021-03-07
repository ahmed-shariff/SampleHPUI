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
    [RequireComponent(typeof(ObjectSelection))]
    public class SpawnHandScrollMenu : HandScrollMenu
    {
        public Selectable floor;
        private ObjectSelection objectSelectionApp;
        private CursorObject cursor;
        private Vector3 currentPosition;

        public ButtonController placeBtn;
        public ButtonController cancelBtn;
        private bool place = false;
        private Transform newObj;

        protected override void Start()
        {
            base.Start();
            placeBtn.contactAction.AddListener(OnPlaceBtn);
            cancelBtn.contactAction.AddListener(OnCancelBtn);
        }

        public virtual void OnSelectionExit()
        {
            if (newObj != null)
                manager.setCurrentObj(newObj);
        }
        
        protected override void OnActivate()
        {
            base.OnActivate();
	    place = false;
	    clearNewObj();
            floor.gameObject.SetActive(true);
            if (objectSelectionApp == null)
            {    
                objectSelectionApp = GetComponent<ObjectSelection>();
                cursor = objectSelectionApp.rayCursor.cursor;
            }
            manager.CurrentObjectChanged += OnCurrentObjectChanged;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            objectSelectionApp.rayCursor.CheckDistanceDelegate = null;
            objectSelectionApp.rayCursor.gameObject.SetActive(false);
            manager.CurrentObjectChanged -= OnCurrentObjectChanged;
            floor.gameObject.SetActive(false);
	    place = false;
	    clearNewObj();
        }
        
        private bool RayCursorCheckDsitance(Selectable s)
        {
            if (s == floor)
                return true;
            return false;
        }
        
        void OnCurrentObjectChanged(Transform t)
        {
            if (place)
            {
                clearNewObj();
                newObj = GameObject.Instantiate(manager.currentObject.GetComponentInChildren<FurnitureFloorBox>(true).gameObject).transform;
                newObj.gameObject.SetActive(true);
            }
	}

        void OnPlaceBtn(ButtonController btn)
        {
            place = !place;
            if (place)
            {
                objectSelectionApp.rayCursor.CheckDistanceDelegate = RayCursorCheckDsitance;
                objectSelectionApp.rayCursor.gameObject.SetActive(true);

                OnCurrentObjectChanged(null);
            }
            else
            {
                objectSelectionApp.rayCursor.CheckDistanceDelegate = null;
                objectSelectionApp.rayCursor.gameObject.SetActive(false);

                clearNewObj();
                newObj = manager.ReplicateCurrentObject().transform;

                var newPosition = currentPosition;
                newPosition.y = manager.currentObject.position.y;
                newObj.position = newPosition;
                newObj.gameObject.SetActive(true);
                
                Debug.Log("Placed object");
                newObj = null;
            }
        }

        void clearNewObj()
        {
            if (newObj != null)
                Destroy(newObj.gameObject);
        }

        void OnCancelBtn(ButtonController btn)
        {
            clearNewObj();
	    place = false;
	    objectSelectionApp.rayCursor.gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();
            if (place)
            {    
                currentPosition = cursor.Position;
                if (newObj != null)
                {
                    currentPosition.y = newObj.position.y;
                    newObj.position = currentPosition;
                }
            }
        }

    }
}
