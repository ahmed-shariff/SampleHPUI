using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[DefaultExecutionOrder(120)]
public class InteractionManger_Caliberation : InteractionManger
{
    public List<ButtonController> order;
    
    private ButtonController selectionBtn;
    // private IEnumerator<ButtonController> btnsIterator;
    private int currentIndex;

    string filePath;
    StreamWriter caliberationDataWriter;
    string outputLine;
    CalibrationProcess calibrationProcess;
    
    // Start is called before the first frame update
    protected override void startAmend()
    {
	if (order.Count > 0)
	{
	    btns = order;
	}
	else
	{
	    var _right_btns = btns.Where(x => x.buttonParentName.EndsWith("r")).ToList().OrderBy(x => (x.fingerParentName.Substring(1, 1) + x.buttonParentName)).ToList();
	    var _center_btns = btns.Where(x => x.buttonParentName.EndsWith("c")).ToList().OrderBy(x => (x.fingerParentName.Substring(1, 1) + x.buttonParentName)).ToList();
	    btns = _right_btns.Concat(_center_btns).ToList();
	}
	Debug.Log(order.Count);
	currentIndex = 0;
	calibrationProcess = new CalibrationProcess();

	foreach(ButtonController btn in btns)
	{
	    btn.button.transform.localScale = btn.button.transform.localScale * 0.65f;
	    btn.contactZone.transform.localScale = getScaledVector(btn.contactZone.transform.localScale, 0.65f);
	    btn.proximalZone.transform.localScale = getScaledVector(btn.proximalZone.transform.localScale, 0.65f);
	}

	selectionBtn = btns[currentIndex];
	selectionBtn.setSelectionHighlight(true);
        selectionBtn.setSelectionDefault(true);
        selectionBtn.invokeDefault();
	
	filePath = getPath("contact"); 
        caliberationDataWriter = new StreamWriter(filePath, true);
        Debug.Log("Writing to:  " + filePath);

    }

    Vector3 getScaledVector(Vector3 t, float size)
    {
	return new Vector3(size * t.x, size * t.y, t.z * 2.5f);
    }


    protected override void processData(string content)
    {
	ConfigData data = JsonConvert.DeserializeObject<ConfigData>(content);

    	outputLine = data.participantId + ", " + data.conditionId + ", ";
    }

    protected override void buttonCallback(ButtonController btn)
    {
	if (btn.isSelectionBtn && (!useSensor || customSubjectHandler.sensorTriggered))
	{
	    StartCoroutine(calibrationProcess.GetCalibrationProcessState(configurationServer));
	    if (calibrationProcess.move != CalibrationProcess.Move.none)
	    {
		var _outputLine = outputLine + ", " + true //valid
		    + ", " + btn.getOutputLine();
		caliberationDataWriter.WriteLine(_outputLine);
		caliberationDataWriter.Flush();

		selectionBtn.setSelectionDefault(false);
		selectionBtn.invokeDefault();

		if (calibrationProcess.move == CalibrationProcess.Move.next)
		    currentIndex++;
		else if (calibrationProcess.move == CalibrationProcess.Move.previous)
		    currentIndex--;

		calibrationProcess.move = CalibrationProcess.Move.none;
		
		if (currentIndex < 0)
		    currentIndex = 0;
		
		if (currentIndex < btns.Count)
		{
		    selectionBtn = btns[currentIndex];
		    selectionBtn.setSelectionDefault(true);
		    selectionBtn.setSelectionHighlight(true);
		    selectionBtn.invokeDefault();
		}
		else
		{
		    StartCoroutine(SendCaliberationData(configurationServer));
		}
	    }
	}
    }

    IEnumerator SendCaliberationData(string uri)
    {
	caliberationDataWriter.Close();
	caliberationDataWriter = null;
	string data = System.IO.File.ReadAllText(filePath);
	WWWForm form = new WWWForm();
        form.AddField("data", data);
        uri += "initconfig";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
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
		}
		else
		{
		    Debug.Log("Succesfully sent data");
		    SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
		}
            }
        }
    }

    private class ConfigData
    {
	public string participantId;
	public string conditionId;
    }

    void OnDestroy()
    {
	Debug.Log("Closing file: " + filePath);
	if (caliberationDataWriter != null)
	    caliberationDataWriter.Close();
    }
}
