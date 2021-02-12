using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Core.DeformableSurfaceDisplay;
using HPUI.Application.Core;
using HPUI.Utils;

namespace HPUI.Application.Sample.InteriorDesign
{
    public class SpawnHandScrollMenu : HandScrollMenu
    {
        public override void OnSelectionExit()
        {
            var newObj = manager.ReplicateCurrentObject().transform;

            var newPosition = spawnPosition.position;
            newPosition += spawnPosition.forward;
            newPosition.y = manager.currentObject.position.y;
            newObj.position = newPosition;
            newObj.gameObject.SetActive(true);
            manager.setCurrentObj(newObj);
	}

    }
}
