﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using System.Linq;
using System.IO;
// using ViconDataStreamSDK.DotNET;
using Newtonsoft.Json;

[DefaultExecutionOrder(120)]
public class InteractionManger : MonoBehaviour
{
    static InteractionManger _instance;
    public static InteractionManger instance
    {
	get
	{
	    if (_instance == null)
		_instance = FindObjectOfType<InteractionManger>();
	    return _instance;
	}
	private set {}
    }
    
    private ButtonController[] buttons;
    private List<ButtonPair> values;
    protected List<ButtonController> btns = new List<ButtonController>();

    public bool useCustomSubjectHandler = true;
    public string configurationServer = "http://127.0.0.1:5000";
    public string defaultData = "{\"buttonSize\": 0.5, \"trialsPerItem\": 5, \"conditionId\": \"dir_50\", \"participantId\": 5}";
    public bool useSensor = true;
    public bool useSelectionSetting = true;
    protected bool configurationComplete;
    
    protected ButtonController winningBtn;
    private float winningValue;
    private bool executeProcess = true;

    public CustomSubjectScript customSubjectHandler;
    private CustomSubjectScript[] subjectScripts;

    public SpriteRenderer feedback;
    public Color sensorTriggerColor = new Color(1, 0.3f, 0.016f, 1);
    public Color successEventColor = Color.yellow;
    private Color defaultColor;
    private bool contactEventTriggerd = false;

    // Start is called before the first frame update
    void Start()
    {
	// var numbers = new List<int>(Enumerable.Range(1, 20));
	// numbers = shuffleList(numbers);
	// Debug.Log("The winning numbers are: " +  string.Join(",  ", numbers.GetRange(0, 20)));
        // Collecting all the button elements that need to be interacted with
        // NOTE: Take care with the indirect case as it can collect those elements only meant for displaying.
	configurationComplete = false;
        subjectScripts = FindObjectsOfType<CustomSubjectScript>();
        values = new List<ButtonPair>();

	if (feedback != null)
	    defaultColor = feedback.color;
	
	getButtons();
        // customSubjectHandler = FindObjectOfType<CustomSubjectScript>();

	startAmend();
        
	if (SceneProperties.instance.useDefaultData)
	{
	    Debug.Log("Using default Data:  " + defaultData);
	    processData(defaultData);
	    configurationComplete = true;
	}
	else
	{
	    StartCoroutine(GetRequest(configurationServer));
	}
    }

    public void getButtons()
    {
	buttons = FindObjectsOfType<ButtonController>();

	btns.Clear();
        foreach (ButtonController btn in buttons)
        {
	    if (btn.transform.root.transform.name == "Right_hand_button_layout" || btn.transform.root.GetComponent<InteractableButtonsRoot>() != null)
	    {
		btn.setValueCallback = setValue;
		btn.postContactCallback = postContactCallback;
		btns.Add(btn);
	    }
        }
	Debug.Log("Got heaps of buttons "  +  btns.Count);
    }

    protected virtual void startAmend()
    {
    }

    protected string getPath(string prefix)
    {
// #if UNITY_EDITOR
//         return Application.dataPath + "/Data/"  + "Saved_Inventory.csv";
//         //"Participant " + "   " + DateTime.now.ToString("dd-MM-yy   hh-mm-ss") + ".csv";
// #elif UNITY_ANDROID
        return Application.persistentDataPath + "/" + prefix + "_data_" + SceneProperties.instance.initTicks + ".csv";
// #elif UNITY_IPHONE
//         return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
// #else
//         return Application.dataPath +"/"+"Saved_Inventory.csv";
// #endif
    }

    IEnumerator GetRequest(string uri)
    {
        uri += "config";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
		        Debug.Log(pages[page] + ": Error: " + uri);
            }
            else
            {
                Debug.Log(pages[page] + ": \nReceived: " + webRequest.downloadHandler.text);
		if (webRequest.error != null && webRequest.error.Contains("404"))
		{
		    Debug.Log("Recived error 404: going to MainScene");
		    SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
		}
		else
		{
		    using(StreamWriter s = new StreamWriter(getPath("scene")))
		    {
			s.WriteLine(webRequest.downloadHandler.text);
		    }
		    processData(webRequest.downloadHandler.text);
		    configurationComplete = true;
		}
            }
        }
    }

    protected virtual void processData(string content)
    {}

    void setValue(float value, ButtonController btn)
    {
        values.Add(new ButtonPair(btn, value));
    }

    void postContactCallback(ButtonController btn)
    {
	if (contactEventTriggerd && processPostContactEventCallback(btn))
	{
	    contactEventTriggerd = false;
	    btn.setSelectionHighlight(false);
	}
    }

    void setButtonState(ButtonController btn, ButtonController.State state)
    {
        if (btn.state != state)
        {
            btn.state = state;
            btn.stateChanged = true;
        }
    }

    void LateUpdate()
    {
	interactionPreProcess();
	if (!useCustomSubjectHandler || (customSubjectHandler && customSubjectHandler.processFrameFlag))
	{
	    if (executeProcess)
	    {
		executeProcess = false;
		// bool did = false;
		preContactLoopCallback();
		foreach (ButtonController btn in btns)
		{
		    buttonCallback(btn);
		    if (btn.stateChanged)
		    {
			switch (btn.state)
			{
			    case ButtonController.State.proximate:
				//btn.proximateAction.Invoke();
				btn.invokeProximate();
				break;
			    case ButtonController.State.contact:
				//btn.contactAction.Invoke();
				processContactEventCallback(btn);
				break;
			    default:
				//btn.defaultAction.Invoke();
				btn.invokeDefault();
				break;
			}
			btn.stateChanged = false;
		    }
		}
		StartCoroutine(Timer());
	    }
	    if (useCustomSubjectHandler)
		customSubjectHandler.processFrameFlag = false;
	    values.Clear();
	}

	if (feedback != null)
	{
	    if (contactEventTriggerd && (!useSensor || customSubjectHandler.sensorTriggered))
		feedback.color = successEventColor;
	    else if (!useSensor || customSubjectHandler.sensorTriggered)
		feedback.color = sensorTriggerColor;
	    else
		feedback.color = defaultColor;
	}
	foreach(CustomSubjectScript subject in subjectScripts)
	{
	    if (subject.enabled)
		subject.writeData();
	}
    }

    private void interactionPreProcess()
    {
	foreach(var btn in btns)
	{
	    btn.processUpdate();
	}
	winningBtn = null;
	winningValue = 1000;
	foreach (ButtonPair entry in values)
	{
	    if (entry.value < winningValue && (!useSelectionSetting || entry.btn.isSelectionBtn))
	    {
		if (winningBtn != null)
		{
		    setButtonState(winningBtn, entry.btn.failedState);
		}
		winningBtn = entry.btn;
		winningValue = entry.value;
	    }
	    else
	    {
		setButtonState(entry.btn, entry.btn.failedState);
	    }
	}

	if (winningBtn && winningBtn.contactDataValid() && (!useSensor || customSubjectHandler.sensorTriggered))
	{
	    // Debug.Log("!!!!!!!  " + winningBtn.buttonParentName + winningBtn.fingerParentName + "  " + winningValue + "  " + btns.Contains(winningBtn));
	    setButtonState(winningBtn, ButtonController.State.contact);
	}
    }

    protected virtual void buttonCallback(ButtonController btn)
    {
    }

    protected virtual void preContactLoopCallback()
    {
    }

    
    protected virtual bool processPostContactEventCallback(ButtonController btn)
    {
	return true;
    }
    
    protected virtual void processContactEventCallback(ButtonController btn)
    {
	invokeContact(btn);
    }

    protected void invokeContact(ButtonController btn)
    {
	btn.invokeContact();
	contactEventTriggerd = true;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        executeProcess = true;
    }

    private class ButtonPair
    {
        public ButtonController btn { get; set; }
        public float value { get; set; }

        public ButtonPair(ButtonController btn, float value)
        {
            this.btn = btn;
            this.value = value;
        }
    }
    
    void OnDestroy()
    {
    }
}
