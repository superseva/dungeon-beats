using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimTest : MonoBehaviour {

    public Animator animator;
    private int walkHash;
	private Dictionary<SequenceCollection.HeroAction, System.Action> heroActions = new Dictionary<SequenceCollection.HeroAction, System.Action>();

	private void Awake()
	{
		heroActions[SequenceCollection.HeroAction.None] = AnimStop;
		heroActions[SequenceCollection.HeroAction.Idle] = AnimStop;
		heroActions[SequenceCollection.HeroAction.MoveForward] = AnimMove;
		heroActions[SequenceCollection.HeroAction.MoveBackwards] = AnimMove;
		heroActions[SequenceCollection.HeroAction.Attack] = AnimAttack;
		heroActions[SequenceCollection.HeroAction.Defend] = AnimDefend;
		heroActions[SequenceCollection.HeroAction.Buff] = AnimBuff;
		heroActions[SequenceCollection.HeroAction.Heal] = AnimHeal;

		animator.SetFloat("IdleOffset", Random.Range(0.0f, 1.0f));
	}

	// Use this for initialization
	private void OnEnable()
	{
		RhythmEvents.OnPlaySequenceStarted += OnPlaySequenceStarted;
	}
	private void OnDIsable()
    {
        RhythmEvents.OnPlaySequenceStarted -= OnPlaySequenceStarted;
    }

	void OnPlaySequenceStarted(SequenceCollection.HeroAction heroAction){
		heroActions[heroAction]();
	}

	public void AnimStop(){
		//if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Hero-Idle" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Hero-None")
                animator.SetTrigger("Stop");
	}

	public void AnimMove()
	{
		animator.SetTrigger("Walk");
	}

    public void AnimAttack()
	{
		animator.SetTrigger("Attack");
		//StartCoroutine(AttackCR());
	}
	private IEnumerator AttackCR()
	{
		yield return new WaitForSeconds(Random.Range(0.0f, 0.75f));
		animator.SetTrigger("Attack");
	}

	public void AnimDefend()
    {
		animator.SetTrigger("Defend");
		//StartCoroutine(DefendCR());
    }
	private IEnumerator DefendCR()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
		animator.SetTrigger("Defend");
    }

	public void AnimBuff()
    {
        animator.SetTrigger("Buff");
    }

	public void AnimHeal()
    {
        animator.SetTrigger("Heal");
    }

    

	// Update is called once per frame
	void Update () {
        //if(Input.GetKeyDown(KeyCode.W)){
        //    animator.SetTrigger("Walk");
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name=="Hero-Walk")
        //        animator.SetTrigger("Stop");
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    animator.SetTrigger("Attack");
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    animator.SetTrigger("Defend");
        //}

	}
}
