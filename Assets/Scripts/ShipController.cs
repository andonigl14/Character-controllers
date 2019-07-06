using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

    // ship acceleration and turn speed
    public float turnSpeed = 0;
    public float acceleration = 0;

    // Maximum ship acceleration and turn speed
    public float TSmax = 0.5f;
    public float Amax = 4.0f;

    // Sail animations
    public Animator anim;
    public bool sail1 = false;
    public bool sail2 = false;

    // Maximum ship acceleration and turn speed
    public float aceleracionMedia = 8;
    public float aceleracionMaxima = 16;

    public float MediumTurnSpeed = 4;
    public float MaxTurnSpeed = 8;

    public Rigidbody rig;
   
    void Start () {}
	
	
	void FixedUpdate ()
    {  // we use fixed update for collisions
        
        if (Input.GetAxis("Horizontal") != 0)
        {
            //Change rigidbody direction instantly to avoid icerink effect
            rig.velocity = transform.forward * rig.velocity.magnitude;

        }
        
        transform.Rotate(Input.GetAxis("Horizontal") * Vector3.up * turnSpeed * Time.fixedDeltaTime);              
        
        //ship movement using rigidbody       
        rig.AddRelativeForce(Input.GetAxis("Vertical")*Vector3.forward *acceleration * Time.fixedDeltaTime, ForceMode.Acceleration);  
              
        
    }


    void Update() // we use update for inputs
    {

        if (Input.GetButtonDown("Fire3"))
        {
            if (!sail1 && !sail2)
            {
                //No Sail --> Main Sail Animations
                anim.Play("Base Layer.Main Sail");
                sail1 = true;

                //we change acceleration and turn speed
                acceleration = aceleracionMedia;
                turnSpeed = MaxTurnSpeed;
            }

            else if (sail1 && !sail2)
            {
                //Main Sail --> Full Sail  Animations
                anim.Play("Base Layer.Full Sail");
                sail2 = true;

                //we change acceleration and turn speed
                acceleration = aceleracionMaxima;
                turnSpeed = MediumTurnSpeed;
            }
        }
        if (Input.GetButtonDown("Fire1"))

        {
            if (sail1 && sail2)
            {
                //Full Sail --> Main Sail  Animations
                anim.Play("Base Layer.Lower Sail");
                sail2 = false;

                //we change acceleration and turn speed
                acceleration = aceleracionMedia;
                turnSpeed = MaxTurnSpeed;

            }

            else if (sail1 && !sail2)
            {
                //Main Sail --> No Sail  Animations
                anim.Play("Base Layer.No Sail");
                sail1 = false;

                //we change acceleration and turn speed
                acceleration = 0;
                turnSpeed = MediumTurnSpeed;
            }
        }
    }
}
