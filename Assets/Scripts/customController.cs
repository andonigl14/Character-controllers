using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class customController : MonoBehaviour {

    //Character Controller
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    //Camera controller
    Vector2 mouseLook;
    Vector2 smoothLook;
    public float sensitivity = 5f;
    public float smoothness = 2f;

    //Weapon controller
    public int weaponAmmo= 60;
    public int weaponMagazine = 20;
    public int magazineSize = 20;
    public float weaponDamage = 30;
    public GameObject bulletHole;
    GameObject bulletAux;
    public Text bulletText;
    public AudioSource shootAudio;
    public AudioSource reloadAudio;
    public ParticleSystem gunFlash;

    void Start () {
        controller =GetComponentInChildren<CharacterController>();
        bulletText.text = weaponMagazine + "/" + weaponAmmo;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {

        cameraControl();
        customMovement();
        weaponManagement();
    }


    void customMovement()
    {
      if (controller.isGrounded)
        {
            
            moveDirection = new Vector3((Input.GetAxis("Horizontal")), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection.normalized); //to walk always in facing direction
                     
            moveDirection = moveDirection * speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);            
                    controller.Move(moveDirection * Time.deltaTime);        
    }

   
    void cameraControl()  //Look around with the camera using the mouse
    {
        Vector2 mouseDir = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDir = Vector2.Scale(mouseDir, new Vector2(sensitivity * smoothness, sensitivity * smoothness));
        smoothLook.x = Mathf.Lerp(smoothLook.x, mouseDir.x, 1f / smoothness);
        smoothLook.y = Mathf.Lerp(smoothLook.y, mouseDir.y, 1f / smoothness);
        mouseLook += smoothLook;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);
        transform.GetChild(0).transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
        
    }


    void weaponManagement()
    {

        //Fire the weapon
        if (Input.GetButtonDown("Fire1") && weaponMagazine > 0)
        {
            // Bit shift the index of the "Damageable " (9) and "scene" layer (10) to get a bit mask
            int layerMask = (1 << 9) | (1 << 10);

            // This would cast rays against all colliders but layer 9 and 10.
            // layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects in the "Damageable" layer
            if (Physics.Raycast(transform.GetChild(0).transform.position, transform.GetChild(0).transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                //bullet decal to see where we have shot
                bulletAux = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                //Do weapon Damage to the enemy in layer 9
                if (hit.transform.gameObject.layer == 9)
                {
                    hit.transform.gameObject.GetComponent<enemyStats>().takeDamage(weaponDamage);
                    bulletAux.transform.parent = hit.transform;
                }
                
                //Debug
                //Debug.DrawRay(transform.GetChild(0).transform.position, transform.GetChild(0).transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
               // Debug.Log("Did Hit");
            }
           
            weaponMagazine--;

            Debug.Log(weaponMagazine + "/" + weaponAmmo);
            bulletText.text = weaponMagazine + "/" + weaponAmmo;
            shootAudio.Play();
            gunFlash.Play();
        }


        //IF we have no bullets play no bullet audio
        if (Input.GetButtonDown("Fire1") && weaponMagazine <= 0)
        {
            reloadAudio.Play();
        }

            //Reload the weapon
            if ((Input.GetButtonDown("Reload") || weaponMagazine<=0) && weaponMagazine != magazineSize)
        {

            if (weaponAmmo >= (magazineSize - weaponMagazine))
            {
                weaponAmmo = weaponAmmo - (magazineSize - weaponMagazine);
                weaponMagazine = magazineSize;
                                                  
            }
            else
            {
                weaponMagazine = weaponMagazine+ weaponAmmo;
                weaponAmmo = 0;
            }
            
            if (weaponAmmo >= 0)
            {
                bulletText.text = weaponMagazine + "/" + weaponAmmo;
                reloadAudio.Play();
            }
           
        }
    }
     
}
