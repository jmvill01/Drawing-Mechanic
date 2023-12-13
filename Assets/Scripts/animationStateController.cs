using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("isRunning", Input.GetAxis("Vertical"));
        animator.SetFloat("isStrafing", Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.LeftShift) && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {    
            animator.SetTrigger("isDashing");
        }

        if (Input.GetKeyDown(KeyCode.CapsLock)) 
        {
            animator.SetBool("isDrawing", true);
        }

        if (Input.GetKeyUp(KeyCode.CapsLock))
        {
            animator.SetBool("isDrawing", false);
        }
    
    }
}
