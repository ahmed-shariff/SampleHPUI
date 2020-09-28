using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TranformMatrixClient : MonoBehaviour
{
    // Start is called before the first frame update
    private TranformMatrixGetter _thread;
    public GameObject ob;

    bool dataAvailable = false;
    string output;
    Dictionary<long, List<float>> data;
    
    private void Start()
    {
        _thread = new TranformMatrixGetter(setTransformMatrixString, getTransformMatrixString);
        _thread.Start();
	ob = new GameObject();
	data = null;
    }

    private void OnDestroy()
    {
        _thread.Stop();
    }

    void Update()
    {
	if (data != null)
	{
	    Debug.Log("Running calculations on  " + data.Keys.Count + " rows");
	    output = "{";
	    foreach(var kpv in data)
	    {
		output += "\"" + kpv.Key + "\": ";
		ob.transform.position = new Vector3(kpv.Value[0], kpv.Value[1], kpv.Value[2]);
		ob.transform.rotation = Quaternion.LookRotation(new Vector3(kpv.Value[6], kpv.Value[7], kpv.Value[8]), new Vector3(kpv.Value[3], kpv.Value[4], kpv.Value[5]));
		output += "[" + 
		    ob.transform.worldToLocalMatrix[0, 0].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[0, 1].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[0, 2].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[0, 3].ToString("F9") + ", " +
		    ob.transform.worldToLocalMatrix[1, 0].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[1, 1].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[1, 2].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[1, 3].ToString("F9") + ", " +
		    ob.transform.worldToLocalMatrix[2, 0].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[2, 1].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[2, 2].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[2, 3].ToString("F9") + ", " +
		    ob.transform.worldToLocalMatrix[3, 0].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[3, 1].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[3, 2].ToString("F9") + ", " + ob.transform.worldToLocalMatrix[3, 3].ToString("F9") + "],";
		// Debug.Log(ob.transform.InverseTransformPoint(ob.transform.position).ToString("F5") + "-- " + ob.transform.worldToLocalMatrix.MultiplyPoint(ob.transform.position));
		// Debug.Log(ob.transform.worldToLocalMatrix.ToString("F5"));
	    }
	    output += "}";
	    data = null;
	    dataAvailable = true;
	}
    }
    
    
    public string getTransformMatrixString()
    {
	if (dataAvailable)
	    return output;
	return null;
    }
    
    public bool setTransformMatrixString(Dictionary<long, List<float>> data)
    {
	if (this.data != null)
	    return false;
	dataAvailable = false;
	this.data = data;
	return true;
    }

    class TranformMatrixGetter: RunAbleThread
    {
	public delegate string GetTransformMatrixString();
	public delegate bool SetTransformMatrixString(Dictionary<long, List<float>> data);

	public GetTransformMatrixString getTransformMatrixString;
	public SetTransformMatrixString setTransformMatrixString;
	
	public TranformMatrixGetter(SetTransformMatrixString setTransformMatrixString, GetTransformMatrixString getTransformMatrixString): base()
	{
	    this.getTransformMatrixString = getTransformMatrixString;
	    this.setTransformMatrixString = setTransformMatrixString;
	}
	
	protected override void Run()
	{
	    ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
	    using (RequestSocket client = new RequestSocket())
	    {
		client.Connect("tcp://130.179.30.160:5555");

		try
		{
		    while (true)
		    {
			// ReceiveFrameString() blocks the thread until you receive the string, but TryReceiveFrameString()
			// do not block the thread, you can try commenting one and see what the other does, try to reason why
			// unity freezes when you use ReceiveFrameString() and play and stop the scene without running the server
			//                string message = client.ReceiveFrameString();
			//                Debug.Log("Received: " + message);
			client.SendFrame("input");
			string output, message = null;
			bool gotMessage = false;
			while (Running)
			{
			    gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
			    if (gotMessage) break;
			}
			Dictionary<long, List<float>> data = JsonConvert.DeserializeObject<Dictionary<long, List<float>>>(message);

			Debug.Log("Received " + "{" + string.Join(",", data.Select(kvp => "[" +kvp.Key + ", " + kvp.Value.Count + "]")) + "}");//data.position[0] + "  " + data.position[1] + "  " + data.position[2] + "  ");

			while(!setTransformMatrixString(data))
			{
			    // Debug.Log("----");
			}
			do{
			    output = getTransformMatrixString();
			} while(output == null);

			Debug.Log("Send:  " + output);
			client.SendFrame(output);

			message = null;
			gotMessage = false;
			while (Running)
			{
			    gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
			    if (gotMessage) break;
			}
			if (gotMessage) Debug.Log("Received2 " + message);
		    }
		}
		catch (Exception e)
		{
		    Debug.Log("Finished with some exception! *poker face*  \n\n" + e);
		}
	    }

	    NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze after one use, not sure why yet
	}
    }
}
