using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPUI.Core;

public class DialScreenApplication : MonoBehaviour
{
    public ButtonController[] btns = new ButtonController[10];
    public ButtonController backspce;
    public TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
	for (int i=0; i< btns.Length; i++)
	{
	    btns[i].id = i;
	    btns[i].contactAction.AddListener(updateText);
	}
	backspce.contactAction.AddListener(backSpace);
    }

    void updateText(ButtonController btn)
    {
	text.text = text.text + btn.id;
    }

    void backSpace(ButtonController btn)
    {
	text.text = text.text.Substring(0, text.text.Length - 1);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
