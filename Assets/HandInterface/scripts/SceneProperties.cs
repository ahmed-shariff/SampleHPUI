using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SceneProperties : MonoBehaviour
{
    static SceneProperties _instance;
    public static SceneProperties instance
    {
	get
	{
	    if (_instance == null)
		_instance = FindObjectOfType<SceneProperties>();
	    return _instance;
	}
	private set {}
    }
    
    string _initTicks;
    public string initTicks
    {
	get
	{
	    if (_initTicks == null)
		_initTicks = "_" + DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
	    return _initTicks;
	}
	private set {}
    }

    public bool useDefaultData = false;
    public bool useTransormfLinkerName = true;
    public bool useRandomization = false;
    
    public long currentTicks {get; private set;}

    // Update is called once per frame
    void Update()
    {
        currentTicks = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
