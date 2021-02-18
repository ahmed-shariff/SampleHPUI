using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;
using HPUI.Application.Core;
using HPUI.Core.DeformableSurfaceDisplay;

namespace HPUI.Application.Sample.InteriorDesign
{
    public class ChangeRoomApp : ApplicationBase
    {
	public DeformableSurfaceDisplayManager deformableSurfaceDisplayManager;
	public Texture2D mainTexture;

        public Transform avatar;

	public Color defaultColor;
	public Color highlightColor;

	public Transform[] positions = new Transform[3];

        int[,] roomCoords;

	protected override void OnActivate()
	{
	    deformableSurfaceDisplayManager.inUse = true;
	    deformableSurfaceDisplayManager.MeshRenderer.material.mainTexture = mainTexture;
            deformableSurfaceDisplayManager.MeshRenderer.material.mainTexture = mainTexture;
            deformableSurfaceDisplayManager.MeshRenderer.material.mainTexture = mainTexture;
	}

	protected override void OnDeactivate()
	{
	    deformableSurfaceDisplayManager.inUse = false;
	}
	
	// Start is called before the first frame update
	void Start()
	{
            if (deformableSurfaceDisplayManager.generatedBtns)
                setupRoomCoords();
            else
                deformableSurfaceDisplayManager.SurfaceReadyAction.AddListener(setupRoomCoords);
	}

        void setupRoomCoords()
        {
            roomCoords = new int [(int)deformableSurfaceDisplayManager.currentCoord.maxX, (int)deformableSurfaceDisplayManager.currentCoord.maxY];
            for (var _x = 0; _x < deformableSurfaceDisplayManager.currentCoord.maxX; _x++)
            {
                for (var _y = 0; _y < deformableSurfaceDisplayManager.currentCoord.maxY; _y++)
                {
                    var row = 0;
                    if (_x / deformableSurfaceDisplayManager.currentCoord.maxX < 0.5)
                        row = 1;
                    if (_y / deformableSurfaceDisplayManager.currentCoord.maxY > 0.5)
                        roomCoords[_x, _y] = 0 + 2 * row;
                    else
                        roomCoords[_x, _y] = 1 + 2 * row;;
                }
            }
        }

	// Update is called once per frame
	void Update()
	{
            if (roomCoords != null)
            {
                if (deformableSurfaceDisplayManager.currentCoord.StateChanged)
                {
                    var id = roomCoords[deformableSurfaceDisplayManager.currentCoord.x, deformableSurfaceDisplayManager.currentCoord.y];
                    avatar.position = positions[id].position;
                    deformableSurfaceDisplayManager.currentCoord.Reset();
                    foreach(var btn in deformableSurfaceDisplayManager.buttonControllers)
                    {
                        int x, y;
                        deformableSurfaceDisplayManager.idToXY(btn.id, out x, out y);
                        if (roomCoords[x, y] == id)
                        {
                            btn.setSelectionDefault(true, highlightColor);
                        }
                        else
                        {
                            btn.setSelectionDefault(true, defaultColor);
                        }
                        btn.invokeDefault();
                    }
                }
            }
        }
    }
}
