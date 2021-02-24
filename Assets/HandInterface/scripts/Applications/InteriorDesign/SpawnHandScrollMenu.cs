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
        public Transform spawnPosition;

        public Selectable floor;
        private ObjectSelection objectSelectionApp;
        private Vector3 currentPosition;

        protected override void Start()
        {
            base.Start();
            floor.OnClosest += SetCurrentFloorPosition;
        }

        protected override void OnActivate()
        {
            floor.gameObject.SetActive(true);
            if (objectSelectionApp == null)
                objectSelectionApp = GetComponent<ObjectSelection>();
            objectSelectionApp.rayCursor.CheckDistanceDelegate = RayCursorCheckDsitance;
            objectSelectionApp.rayCursor.gameObject.SetActive(true);
            manager.CurrentObjectChanged += OnCurrentObjectChanged;
        }

        protected override void OnDeactivate()
        {
            objectSelectionApp.rayCursor.CheckDistanceDelegate = null;
            objectSelectionApp.rayCursor.gameObject.SetActive(false);
            manager.CurrentObjectChanged -= OnCurrentObjectChanged;
            floor.gameObject.SetActive(false);
        }
        
        private bool RayCursorCheckDsitance(Selectable s)
        {
            if (s == floor)
                return true;
            return false;
        }
        
        void OnCurrentObjectChanged(Transform t)
        {
            var newObj = manager.ReplicateCurrentObject().transform;

            var newPosition = spawnPosition.position;
            newPosition += spawnPosition.forward;
            newPosition.y = manager.currentObject.position.y;
            newObj.position = newPosition;
            newObj.gameObject.SetActive(true);
            manager.setCurrentObj(newObj);
	}

        void SetCurrentFloorPosition(Selectable s)
        {
            if (s == floor)
            {
                currentPosition = objectSelectionApp.rayCursor.cursor.Position;
            }
        }

    }
}
