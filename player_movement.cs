using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    [SerializeField] public float MoveSpeed = 4;
    [SerializeField] public float Jump = 100;
    public Health health;

    [SerializeField] Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //inputs
        float horInput = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        float verInput = Input.GetAxisRaw("Vertical") * MoveSpeed;

        //camera direction
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        //creating relative camera directions
        Vector3 forwardRelative = verInput * camForward;
        Vector3 rightRelative = horInput * camRight;

        Vector3 moveDir = (forwardRelative + rightRelative).normalized * MoveSpeed;

        //movement
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0))
        {
            Vector3 dodgeDirection = transform.forward; // or use Camera.main.transform.forward if you prefer camera-facing dodge
            dodgeDirection.y = 0; // make sure it's horizontal only
            dodgeDirection.Normalize();

            rb.velocity = dodgeDirection * Jump; // 'Jump' now represents dodge speed

            animator.SetTrigger("Dodge");
        }

        if (rb.velocity.magnitude > 0.1) transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        animator.SetFloat("RunningSpeed", Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) );
    }
}
