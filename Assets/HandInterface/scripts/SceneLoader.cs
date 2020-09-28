using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SceneLoader : MonoBehaviour
{
    public string experimentServer = "http://127.0.0.1:5000/";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Done and loading");
        StartCoroutine(GetRequest(experimentServer));
    }

    IEnumerator GetRequest(string uri)
    {
        uri += "next";
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
                Data data = JsonConvert.DeserializeObject<Data>(webRequest.downloadHandler.text);
		if (data.sceneName != "MainScene")
		{
		    // SceneManager.LoadScene(data.sceneName, LoadSceneMode.Single);
		    StartCoroutine(Timer(data.sceneName));
		}
            }
        }
    }

    IEnumerator Timer(string sceneName)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    class Data
    {
        public string sceneName;
    }

}
