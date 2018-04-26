using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;
using DG.Tweening;


public class PlayHead : MonoBehaviour {


    Rigidbody2D rigidbody;
    public float maxSpeed = 1;

    bool grounded = false;
    public Transform groundCheck;
    float groundradius = 0.2f;
    public LayerMask whatIsGround;
    float move = 0;
    Tween tween;

    private BeatObserver beatObserver;

    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        beatObserver = GetComponent<BeatObserver>();
        
    }

    void Update()
    {
        if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {

            move = 1;
           
        }
        if ((beatObserver.beatMask & BeatType.UpBeat) == BeatType.UpBeat)
        {
            move = 0;
        }
    }


    float gravityModifier = 1f;
    // Update is called once per frame
    void FixedUpdate ()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundradius, whatIsGround);


        //float move = Input.GetAxis("Horisontal");
        //move = 1;
        rigidbody.velocity = new Vector2(move * maxSpeed, rigidbody.velocity.y);
        rigidbody.velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    }
}
