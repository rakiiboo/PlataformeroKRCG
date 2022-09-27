using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody2D;
    private float horizontal;
    private Animator animator;
    public float jumpForce;
    private bool isGrounded;
    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal") * speed;
        if (horizontal < 0.0f)
        {
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
        else if (horizontal > 0.0f)
        {
            transform.localScale = new Vector2(1.0f, 1.0f);
        }

        animator.SetBool("isRunning", horizontal != 0.0f);

        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.blue);
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.1f))
        {
            isGrounded = true;
             animator.SetBool("isJumping", false);

        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        DeathOnFall();
    }

   public void Death(){
transform.position = initialPosition;
   }

    private void Jump(){
         animator.SetBool("isJumping", true);
        rigidbody2D.AddForce(Vector2.up * jumpForce);
    }

private void DeathOnFall(){
    if(transform.position.y<-10f){
        Death();
    }
}

    private void FixedUpdate(){
        rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
    }

}
