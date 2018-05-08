using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class SequencerUI : MonoBehaviour {

    public static SequencerUI instance;
    public static bool opened = false;
    
    public LayerMask layerMask = Physics.DefaultRaycastLayers;

	private void Awake()
	{
        if(instance==null){
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }else if(instance!=this){
            Destroy(gameObject);
        }
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(selected){
            selected.anchoredPosition += ziFinger.ScreenDelta;
            Debug.Log(selected.anchoredPosition);
        }
	}

    void OnShowUI()
    {
        opened = true;
    }

    void OnHideUI()
    {
        opened = false;
    }

    RectTransform selected;
    LeanFinger ziFinger;

    public void OnFingerTouch(LeanFinger finger){

        var component = default(Component);
        var results = LeanTouch.RaycastGui(finger.ScreenPosition, layerMask);

        if (results != null && results.Count > 0)
        {
            selected = results[0].gameObject.GetComponent<RectTransform>();
            ziFinger = finger;
            Debug.Log(selected.anchoredPosition);
            Debug.Log(finger.ScreenDelta);
        }
    }
    public void OnFingerUp(LeanFinger finger){
        if(finger.Index==ziFinger.Index){
            selected = null;
            ziFinger = null;
        }
    }
}
