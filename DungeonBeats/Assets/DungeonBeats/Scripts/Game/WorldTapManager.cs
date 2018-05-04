using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class WorldTapManager : MonoBehaviour {

    public LayerMask layerMask = Physics.DefaultRaycastLayers;
	// Use this for initialization
	void Start () {
        
	}


    private void MosterTap(Monster monster){
        monster.ClickOnBeat();
    }
    

    public void OnWorldFingerDown(LeanFinger finger)
    {
        //var camera = LeanTouch.GetCamera(Camera, gameObject);

        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
            var hit = default(RaycastHit);

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask))
            {
                if(hit.transform.gameObject.CompareTag("Monster")){
                    MosterTap(hit.transform.gameObject.GetComponent<Monster>());
                }
               
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
