using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class CalibrationProcess
{
    
    public enum Move
    {
	none,
	next,
	previous,
	done,
    }
    public Move move;

    public CalibrationProcess()
    {
	this.move = Move.none;
    }

    public IEnumerator GetCalibrationProcessState(string uri)
    {
        uri += "initconfig";
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
                // Debug.Log(pages[page] + ": \nReceived: " + webRequest.downloadHandler.text);
		if (webRequest.error != null && webRequest.error.Contains("404"))
		{
		    Debug.Log("Recived error 404: going to MainScene");
		}
		else
		{
		    GetData data = JsonConvert.DeserializeObject<GetData>(webRequest.downloadHandler.text);
		    if (data.move == 1)
			move = Move.next;
		    else if (data.move == 2)
		    {
			Debug.Break();
		    }
		    else if (data.move == 3)
		    {
			move = Move.previous;
		    }
		    else if (data.move == 4)
		    {
			move = Move.done;
		    }
		}
            }
        }
    }

    private class GetData
    {
        public int move;
    }
}
