using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundScroller : MonoBehaviour {
    
	public float steps = 2;

	private BoxCollider boxCollider;
	private float width;
	private float movementDistance;
	private float totalDistance = 0;
	float endPoint = 0;

	// Use this for initialization
	void Awake () {
		boxCollider = gameObject.GetComponent<BoxCollider>();
		width = boxCollider.size.x;
		movementDistance = width/steps;
	}

	private void OnEnable()
	{
		RhythmEvents.OnPlaySequenceStarted += OnPlaySequenceStarted;
	}
	private void OnDisable()
	{
		RhythmEvents.OnPlaySequenceStarted -= OnPlaySequenceStarted;
	}

	void OnPlaySequenceStarted(SequenceCollection.HeroAction heroAction){
		if(heroAction==SequenceCollection.HeroAction.MoveForward){
			float time = TempoUtils.instance.GetBeatInSeconds() * 4;
			//transform.DOMoveX(transform.position.x - movementDistance, time).SetEase(Ease.InOutSine);
			endPoint =totalDistance+ movementDistance; 
			DOTween.To(() => totalDistance, x => totalDistance = x, endPoint, time).SetEase(Ease.Linear);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(0- Mathf.Repeat(totalDistance, width), transform.position.y, transform.position.z);
	}
}
