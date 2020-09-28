using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapApplication : MonoBehaviour
{
    public BtnMapperStatic btnMapperStatic;
    public SpriteRenderer spriteRenderer;

    public Sprite[] sprites = new Sprite[2];
    
    // Start is called before the first frame update
    void Start()
    {
	
    }

    // Update is called once per frame
    void Update()
    {
	if (btnMapperStatic.currentCoord.x == 0 && btnMapperStatic.currentCoord.y == 0)
	{
	    spriteRenderer.gameObject.SetActive(false);
	}
	else
	{
	    spriteRenderer.gameObject.SetActive(true);
	    
	    if ((btnMapperStatic.currentCoord.x / btnMapperStatic.currentCoord.maxX) > 0.7)
		spriteRenderer.sprite = sprites[0];
	    else
		spriteRenderer.sprite = sprites[1];
	}
    }
}
