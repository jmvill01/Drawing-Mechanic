using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
 // Editor variables ---------------
    [Header("Live Variables:")]
    public int speed;

    [Header("General Vars:")]
    public int defSpeed = 5;
    public int sprintSpeed = 5;
    public int crouchSpeed = 5;
    public int drawSpeed = 2;
    public int jumpMultiplier = 5;
    public bool isMovementEnabled = true;


    [Header("Camera Vars:")]
    public int Sensitivity = 0;

    // Private variables ---------------
    bool inSprint;
    Rigidbody rigBodyComp;

        // Camera variables ----------
        GameObject mainCamera;
        string xAxis = "Mouse X";
        Quaternion camAngle;

    // Jumping variables
    bool jumpPressed;
    bool isGrounded;

        // Dashing variables
        float animationTimer;
        public int dashForceVert, dashForceHorz;
        public float defCharDrag, defCharAngDrag;
        public float dashDrag;
        float floorDrag, angularDrag;
        Rigidbody rbPlayer;
        Vector3 dashDirection;
        int dashCounter;

    // Start is called before the first frame update
    void Start()
    { 
        speed = defSpeed;
        rigBodyComp = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        rbPlayer = gameObject.GetComponent<Rigidbody>();
        dashCounter = 0;
        floorDrag = angularDrag = dashDrag;
        // rbPlayer.gr
    }

    // Update is called once per frame
    void Update()
    {

        // Engages Jump
        if (Input.GetKeyDown(KeyCode.Space) && isMovementEnabled)
        {
            // Debug.Log("Space key pressed!");
            jumpPressed = true;
        }

        // Engages Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
        {
            Sprint();
        }

        // Stops sprint
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetAxis("Vertical") <= 0)
        {
            StopSprint();
        }

        // Start Drawing
        if (Input.GetKeyDown(KeyCode.CapsLock)) 
        {
            StartDrawing();
        }

        // Stop Drawing
        if (Input.GetKeyUp(KeyCode.CapsLock))
        {
            StopDrawing();
        }

        // Start Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashCounter == 0) 
            {
                Debug.Log("Dashing!");
                dashDirection = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
                dashDirection = transform.TransformDirection(dashDirection);
                rbPlayer.drag = floorDrag;
                rbPlayer.angularDrag = angularDrag;
                rbPlayer.AddForce(transform.up * dashForceVert, ForceMode.Impulse);
                rbPlayer.AddForce(dashDirection * dashForceHorz, ForceMode.Impulse);

            }

            
            dashCounter += 1;
        }
      

        if (Input.GetKey(KeyCode.LeftShift))
        {
            StartDashing();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dashCounter = 0;
            rbPlayer.drag = 1;
            rbPlayer.angularDrag = 1;
            Debug.Log("Out of Dash.");
        }

        // Stop Dashing

        // Camera Rotation
        if (isMovementEnabled)
        {
            // Debug.Log(Input.GetAxis(xAxis));
            this.transform.Rotate(0, Input.GetAxis(xAxis) * Sensitivity, 0);
        }
        
       
    }

    void FixedUpdate()
    {
        IsGrounded();

        // Performs character jump
        if (jumpPressed && isGrounded && isMovementEnabled)
        {
            rigBodyComp.AddForce(Vector3.up * jumpMultiplier, ForceMode.VelocityChange);
            jumpPressed = false;
        }



        // Performs character movement
        if (isMovementEnabled)
        {
            this.transform.position = transform.position + (gameObject.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime)
                                                    + (gameObject.transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        }
        
    }

    private void Sprint()
    {
        speed = sprintSpeed;
        inSprint = true;
    }

    private void StopSprint()
    {
        speed = defSpeed;
        inSprint = false;
    }

    public void IsGrounded() 
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, (transform.localScale.y + .1f));

        // if (inJump && isGrounded) 
        // {
        //     inJump = false;
        //     allowControl = true;
        // }

    }

    public void StartDrawing()
    {
        speed = drawSpeed;

    }

    public void StopDrawing()
    {
        speed = defSpeed;

    }

    public void StartDashing()
    {
        // Debug.Log("In Dash...");
        floorDrag = Mathf.Lerp(floorDrag, defCharDrag, .001f);
        rbPlayer.drag = floorDrag;
        angularDrag = Mathf.Lerp(angularDrag, defCharAngDrag, .001f);
        rbPlayer.angularDrag = angularDrag;


    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
    }
}
