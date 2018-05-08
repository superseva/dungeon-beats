using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceLine : MonoBehaviour {
    public SequenceCollection.HeroAction heroAction;
    public List<SequenceBeatMarker> beatMarkers;
    public Text actionNameText;

    private AudioSource audioSource;
    private List<int> sequence;
    private SequenceBeatMarker beatMarker;

	void Start () {
        actionNameText.text = heroAction.ToString();
        DrawSequence();
	}

    private void DrawSequence(){
        sequence = SequenceCollection.instance.ALL_SEQUENCES[heroAction];
        for (int i = 0; i < sequence.Count; i++){
            beatMarker = beatMarkers[i];
            beatMarker.SetMark(sequence[i]);
        }
    }

    public void OnTapMarker(int index){
        Debug.Log(heroAction.ToString()+ " ... " + index);
        beatMarker = beatMarkers[index];
        if (beatMarker.markNo < 2)
            beatMarker.SetMark(beatMarker.markNo + 1);
        else
            beatMarker.SetMark(0);
    }
	
	void Update () {
		
	}
}
