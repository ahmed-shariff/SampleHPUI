using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(110)]
public class ButtonController : MonoBehaviour
{

    public delegate void SetValue(float value, ButtonController btn);
    public delegate void PostContactCallback(ButtonController btn);

    public bool initialized {get; private set;} = false;
    
    public ButtonZone proximalZone;
    public ButtonZone contactZone;

    [SerializeField]
    public RelativePosition relativePosition = RelativePosition.none;

    public float randomYMax = 0.0f;
    public float randomYMin = 0.0f;

    private float randomDiff;
    private bool validRandomDiff;
    private Vector3 defaultPosition;
    private float randomValue;

    public ButtonControllerEvent proximateAction = new ButtonControllerEvent();
    public ButtonControllerEvent contactAction = new ButtonControllerEvent();
    public ButtonControllerEvent defaultAction = new ButtonControllerEvent();

    public SetValue setValueCallback { private get; set; }
    public PostContactCallback postContactCallback {private get; set;}

    [System.NonSerialized]
    public bool stateChanged = false;

    private State _state;
    public State previousState { get; private set; }

    public string fingerParentName { get; private set; }
    public string buttonParentName { get; private set; }

    // [System.NonSerialized]
    public int id = -1;

    [SerializeField]
    public State failedState = State.proximate;

    public State state
    {
        get
        {
            return _state;
        }
        set
        {
	    previousState = _state;
            _state = value;
	    if (previousState == State.contact && postContactCallback != null )
	    {
		postContactCallback(this);
	    }
        }
    }

    ButtonColorBehaviour colbe;
    ButtonScaleBehaviour scabe;
    bool _isSelection = false;

    [System.NonSerialized]
    public SpriteRenderer button;
    
    public bool isSelectionBtn
    {
	get{
	    return _isSelection;
	}
	private set{
	    // Debug.Log("--- " + value);
	    // transform.parent.gameObject.SetActive(value);
	    _isSelection = value;

	    if (_isSelection)
	    {
		if (SceneProperties.instance.useRandomization && validRandomDiff)
		{
		    randomValue = UnityEngine.Random.Range(randomYMin, randomYMax);
		    this.transform.parent.localPosition = new Vector3(defaultPosition.x, randomValue, defaultPosition.z);
		    colbe.spriteRenderer.transform.parent.localPosition = this.transform.parent.localPosition;
		}
	    }
	    else
	    {
		if (SceneProperties.instance.useRandomization && validRandomDiff)
		{
		    // this.transform.parent.localPosition = defaultPosition;
		    // colbe.spriteRenderer.transform.parent.localPosition = defaultPosition;
		}
		
	    }
	}
    }

    public enum State
    {
        outside,
        proximate,
        contact
    }

    public enum RelativePosition
    {
	onSkin,
	offSkin,
	none
    }

    public bool contactRecieved()
    {
        return state == State.contact;
    }

    //public void keepContactState(bool keepContactState)
    //{
    //    Debug.Log("Got ---  " + keepContactState + "  " + state + "  " + GetComponentInParent<TransformLinker>().parent.name);
    //    if (!keepContactState && state == State.contact)
    //    {
    //        state = State.proximate;
    //        stateChanged = true;
    //    }
    //    else if(keepContactState)
    //    {
    //        state = State.contact;
    //        stateChanged = true;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        state = State.outside;
        scabe = GetComponent<ButtonScaleBehaviour>();
        colbe = GetComponent<ButtonColorBehaviour>();
	isSelectionBtn = false;
	button = colbe.spriteRenderer;

	defaultPosition = this.transform.parent.localPosition;

	if (SceneProperties.instance.useTransormfLinkerName)
	{
	    var tl = GetComponentInParent<TransformLinker>();
	    fingerParentName = tl.parent.name;
	    buttonParentName = tl.transform.name;
	}
	else
	{
	    fingerParentName = this.transform.parent.parent.name;
	    buttonParentName = this.transform.parent.name;
	}

	if (randomYMax <= randomYMin)
	{
	    if (randomYMax < randomYMin)
		Debug.LogError("random Y max cannot be samller than random Y min");
	    randomDiff = 0;
	    validRandomDiff = false;
	}
	else
	{
	    randomDiff = randomYMax - randomYMin;
	    validRandomDiff = true;
	}

	if (!proximalZone.gameObject.activeSelf)
	    proximalZone = null;
	
	initialized = true;
    }

    public void resetStates()
    {
	_state = State.outside;
       previousState = State.outside;
       contactZone.state = ButtonZone.State.outside;
       if (proximalZone != null)
	   proximalZone.state = ButtonZone.State.outside;
    }

    // Update is called once per frame
    // void Update()
    public void processUpdate()
    {
	if (contactZone.state == ButtonZone.State.inside)
        {
            if (state != State.contact)
            {
                stateChanged = true;
            }
            // state = State.contact;
            // Debug.Log("++++++++++++++++++++++++++++++++++  Got ---  " + state + "  " + buttonParentName); //GetComponentInParent<TransformLinker>().parent.name);
            setValueCallback((contactZone.colliderPosition - this.transform.position).magnitude, this);
        }
        else if (proximalZone != null && proximalZone.state == ButtonZone.State.inside)
        {
            if (state != State.proximate)
                stateChanged = true;
            state = State.proximate;
        }
        else
        {
            if (state != State.outside)
                stateChanged = true;
            state = State.outside;
        }
    }

    public string getStreamLine()
    {
	var tl = GetComponentInParent<TransformLinker>();
	return SceneProperties.instance.currentTicks + ", " +
	    fingerParentName + ", " + buttonParentName + ", " +
	    tl.transform.position.x + ", " + tl.transform.position.y + ", " + tl.transform.position.z;
    }

    public string  getOutputLine()
    {
	var dp = this.transform.parent.TransformPoint(defaultPosition);
        return SceneProperties.instance.currentTicks + ", " +
	    fingerParentName + ", " + buttonParentName + ", " + id + ", " +
	    contactZone.colliderPosition.x + ", " + contactZone.colliderPosition.y + ", " + contactZone.colliderPosition.z + ", " +
	    contactZone.selfPosition.x + ", " + contactZone.selfPosition.y + ", " + contactZone.selfPosition.z + ", " +
	    contactZone.contactPoint.x + ", " + contactZone.contactPoint.y + ", " + contactZone.contactPoint.z + ", " +
	    contactZone.contactPlanePoint.x + ", " + contactZone.contactPlanePoint.y + ", " + contactZone.contactPlanePoint.z + ", " +
	    contactZone.worldToLocalMatrix[0, 0] + ", " + contactZone.worldToLocalMatrix[0, 1] + ", " + contactZone.worldToLocalMatrix[0, 2] + ", " + contactZone.worldToLocalMatrix[0, 3] + ", " +
	    contactZone.worldToLocalMatrix[1, 0] + ", " + contactZone.worldToLocalMatrix[1, 1] + ", " + contactZone.worldToLocalMatrix[1, 2] + ", " + contactZone.worldToLocalMatrix[1, 3] + ", " +
	    contactZone.worldToLocalMatrix[2, 0] + ", " + contactZone.worldToLocalMatrix[2, 1] + ", " + contactZone.worldToLocalMatrix[2, 2] + ", " + contactZone.worldToLocalMatrix[2, 3] + ", " +
	    contactZone.worldToLocalMatrix[3, 0] + ", " + contactZone.worldToLocalMatrix[3, 1] + ", " + contactZone.worldToLocalMatrix[3, 2] + ", " + contactZone.worldToLocalMatrix[3, 3] + ", " +
	    contactZone.selfScale.x + ", " + contactZone.selfScale.y + ", " + contactZone.selfScale.z + ", " +
	    contactZone.selfForward.x + ", " + contactZone.selfForward.y + ", " + contactZone.selfForward.z + ", " +
	    contactZone.colliderScale.x + ", " + contactZone.colliderScale.y + ", " + contactZone.colliderScale.z + ", " +
	    contactZone.colliderSurfacePoint.x + ", " + contactZone.colliderSurfacePoint.y + ", " + contactZone.colliderSurfacePoint.z + ", " +
	    contactZone.parentWorldToLocalMatrix[0, 0] + ", " + contactZone.parentWorldToLocalMatrix[0, 1] + ", " + contactZone.parentWorldToLocalMatrix[0, 2] + ", " + contactZone.parentWorldToLocalMatrix[0, 3] + ", " +
	    contactZone.parentWorldToLocalMatrix[1, 0] + ", " + contactZone.parentWorldToLocalMatrix[1, 1] + ", " + contactZone.parentWorldToLocalMatrix[1, 2] + ", " + contactZone.parentWorldToLocalMatrix[1, 3] + ", " +
	    contactZone.parentWorldToLocalMatrix[2, 0] + ", " + contactZone.parentWorldToLocalMatrix[2, 1] + ", " + contactZone.parentWorldToLocalMatrix[2, 2] + ", " + contactZone.parentWorldToLocalMatrix[2, 3] + ", " +
	    contactZone.parentWorldToLocalMatrix[3, 0] + ", " + contactZone.parentWorldToLocalMatrix[3, 1] + ", " + contactZone.parentWorldToLocalMatrix[3, 2] + ", " + contactZone.parentWorldToLocalMatrix[3, 3] + ", " +
	    this.transform.parent.localPosition.x + ", " + this.transform.parent.localPosition.y + ", " + this.transform.parent.localPosition.z + ", " +
	    defaultPosition.x + ", " + defaultPosition.y + ", " + defaultPosition.z + ", " + randomValue + ", ";
    }

    public bool contactDataValid()
    {
	var r = (contactZone.colliderSurfacePoint - contactZone.colliderPosition).magnitude;
	var p = (contactZone.contactPlanePoint - contactZone.colliderPosition).magnitude;
	var condition1 = (r * 0.99f) > p;
	var colliderLocalPosition = contactZone.worldToLocalMatrix.MultiplyPoint(contactZone.colliderPosition);
	bool condition2 = colliderLocalPosition.x <= 2.5f && colliderLocalPosition.y <= 2.5f && colliderLocalPosition.x >= -2.5f && colliderLocalPosition.y >= -2.5f;
	// Debug.Log(condition1 && condition2);
	// Debug.Log(r + " - " + p + " = " + (r-p) + "   %:" + (p/r) + "               " + buttonParentName + "_" + fingerParentName);
	// Debug.Log(contactZone.worldToLocalMatrix.MultiplyPoint(contactZone.colliderPosition).ToString("F6"));
	return condition1;// && condition2;
    }

    public void replicateObject(string suffix)
    {
	contactZone.replicateObject(SceneProperties.instance.currentTicks.ToString() + "_" + fingerParentName + buttonParentName + "_" + suffix);
    }
    
    //public void ExecuteButtonAction()
    //{
    //    if (state != State.outside)
    //        Debug.Log("p::::::" + GetComponentInParent<TransformLinker>().parent.name + "   " + state + "  " + stateChanged);
    //    if (stateChanged)
    //    {
    //        switch (state)
    //        {
    //            case State.proximate:
    //                proximateAction.Invoke();
    //                break;
    //            case State.contact:
    //                //Debug.Log("Syched-------------------" + GetComponentInParent<TransformLinker>().parent.name);
    //                contactAction.Invoke();
    //                break;
    //            default:
    //                defaultAction.Invoke();
    //                break;
    //        }
    //    }
    //}

    public void invokeProximate()
    {
        proximateAction.Invoke(this);
        colbe.resetColor();
        // scabe.resetScale();
    }

    public void invokeDefault()
    {
        defaultAction.Invoke(this);
        colbe.resetColor();
        // scabe.resetScale();
    }

    public void invokeContact()
    {
        contactAction.Invoke(this);
        colbe.invokeColorBehaviour();
        // scabe.invokeScaleBehaviour();
    }

    public void setSelectionDefault(bool selection)
    {
        isSelectionBtn = selection;
        colbe.setSelectionDefault(selection);
    }

    public void setSelectionDefault(bool selection, Color color)
    {
        isSelectionBtn = selection;
        colbe.setSelectionDefault(selection, color);
    }

    public void setSelectionHighlight(bool selection)
    {
        colbe.setSelectionHighlight(selection);
    }
}

[Serializable]
public class ButtonControllerEvent : UnityEvent<ButtonController> {}
