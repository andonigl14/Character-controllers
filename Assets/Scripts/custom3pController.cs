using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class custom3pController : MonoBehaviour {

    //Character Controller
    public float health = 100.0f;
    public float speed = 3.0f;
    public float runSpeed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    float heading;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Animator playerAnim;
    private bool isDead = false;

    //UI

    public InputField UIspeed;
    public InputField UIrunSpeed;
    public InputField UIGravity;
    public InputField UIhealth;
    public InputField UIjumpHeight;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();

        //inicilize ui with current parameters
        UIspeed.text = speed.ToString();
        UIhealth.text = health.ToString();
        UIjumpHeight.text = jumpSpeed.ToString();
        UIGravity.text = gravity.ToString();
        UIrunSpeed.text = runSpeed.ToString();

        //adding listeners to update parameter on input change
        UIspeed.onValueChanged.AddListener(delegate { UpdateParameters(); });
        UIhealth.onValueChanged.AddListener(delegate { UpdateParameters(); });
        UIjumpHeight.onValueChanged.AddListener(delegate { UpdateParameters(); });
        UIrunSpeed.onValueChanged.AddListener(delegate { UpdateParameters(); });
        UIGravity.onValueChanged.AddListener(delegate { UpdateParameters(); });

    }
	
	// Update is called once per frame
	void Update () {
        Custom3pMovement();
    }


    void Custom3pMovement() //there might be some problem with Time.deltatime, gotta check it
    {
  
        if (controller.isGrounded && isDead == false)
        {

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            //moveDirection = transform.TransformDirection(moveDirection.normalized);

            if(Mathf.Abs(Input.GetAxis("Horizontal"))>=0.7f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.7f)
                moveDirection = moveDirection * runSpeed;
            else
                moveDirection = moveDirection * speed ;


            heading = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));//returns de angle of the joystick

            // only change the characters angle if we are moving, this way the character does not face foward when we are no using the joystick
            if (heading != 0 || (heading == 0 && Input.GetAxis("Vertical") > 0)) 
                transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);

            playerAnim.SetFloat("VelX", Input.GetAxis("Horizontal"));
            playerAnim.SetFloat("VelY", Input.GetAxis("Vertical"));

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                playerAnim.SetBool("jump", true);
            }
            else
            {
                playerAnim.SetBool("jump", false);
            }  

            if (Input.GetButton("Fire1"))
            {
                playerAnim.SetTrigger("attack");
            }
            
            if (health <= 0)
            {
                playerAnim.SetBool("dead", true);
                isDead = true;
            }                        
        }
      
        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity*Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);        
    }

    // this is called by the listeners to change the parameters
    void UpdateParameters()
    {
        if (UIspeed.text != "")
             speed = float.Parse(UIspeed.text);

        if (UIhealth.text != "")
            health = float.Parse(UIhealth.text);

        if (UIjumpHeight.text != "")
            jumpSpeed = float.Parse(UIjumpHeight.text);

        if (UIGravity.text != "")
            gravity = float.Parse(UIGravity.text);

        if (UIrunSpeed.text != "")
            runSpeed = float.Parse(UIrunSpeed.text);
    }
}
