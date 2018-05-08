using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceBeatMarker : MonoBehaviour {

    public int index;
    public int markNo;
    private Image img;

	private void Awake()
	{
        img = gameObject.GetComponent<Image>();
	}

	private void Start()
	{
        

	}

	public void SetMark(int _markNo  = -1){
        markNo = _markNo;
        if(markNo==0 || markNo == -1){
            img.color = Color.white;
        }else if(markNo == 1){
            img.color = Color.blue;
        }else if(markNo == 2){
            img.color = Color.green;
        }
    }



}
