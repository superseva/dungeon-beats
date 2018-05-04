using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Text actionText;
    public GameObject semafor;
    public List<Image> semaforImages;

	private void OnEnable()
	{
        RhythmEvents.OnRecordingStarted += OnRecordingStarted;
        RhythmEvents.OnRecordingCompleted += OnRecordingCompleted;
        RhythmEvents.OnBeatRecorded += OnBeatRecorded;
        RhythmEvents.OnPlaySequenceStarted += OnPlaySequenceStarted;
	}

	private void OnDisable()
	{
        RhythmEvents.OnRecordingStarted -= OnRecordingStarted;
        RhythmEvents.OnRecordingCompleted -= OnRecordingCompleted;
        RhythmEvents.OnBeatRecorded -= OnBeatRecorded;
        RhythmEvents.OnPlaySequenceStarted -= OnPlaySequenceStarted;
	}

	// Use this for initialization
	void Start () {
        semafor.SetActive(false);
	}

    private void OnRecordingStarted()
    {
        ResetImages();
        semafor.SetActive(true);
        actionText.text = string.Empty;
    }

    private void OnRecordingCompleted()
    {
        semafor.SetActive(false);
    }

    private void OnBeatRecorded(int beatInSequence, int finger)
    {
        semaforImages[beatInSequence].color = (finger == 1)?Color.blue:Color.green;
        //Debug.Log("BEAT INDEX :" + beatInSequence+ ", finger:" + finger);
    }

    private void ResetImages(){
        foreach (Image img in semaforImages)
        {
            img.color = Color.white;
        }
    }

    private void OnPlaySequenceStarted(SequenceCollection.HeroAction heroAction){
        actionText.text = heroAction.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
