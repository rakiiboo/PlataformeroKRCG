using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public int life = 2;

    public float speed;

    public float jumpForce;

    public GameObject bulletPrefab;

    public GameObject lifesPanel;

    public Vector2 respawnpoint;

    public float horizontal;

    public float cooldownTime = 1f;

    private Rigidbody2D rigidBody2D;

    private bool isGrounded;

    private bool isInCooldown;

    private Animator animator;

    private Vector2 initialPosition;

    private float lastShoot;

    private GameObject warpA;

    private GameObject warpA;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        respawnpoint = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            // if (isGrounded)
            // {
            //     respawnpoint = new Vector2(transform.position.x, transform.position.y+2);
            // }
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

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if (!isGrounded)
            {
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }

            if (Input.GetKeyDown(KeyCode.E) && Time.time > lastShoot + 1f)
            {
                Shoot();
                lastShoot = Time.time;
            }

            DeathOnFall();
        }
    }

    public void Death()
    {
        transform.position = initialPosition;
        respawnpoint = initialPosition;
        if (life <= 0)
        {
            life = 2;
            for (int i = 0; i < lifesPanel.transform.childCount; i++)
            {
                lifesPanel.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void Hit(float knockback, GameObject enemy)
    {

        if (!isInCooldown)
        {
            StartCoroutine(cooldown());
            if (life > 0)
            {
                lifesPanel.transform.GetChild(life).gameObject.SetActive(false);
                life -= 1;
                if (enemy)
                {
                    Vector2 difference =
                        (transform.position - enemy.transform.position);
                    float knockbackDirection=difference.x >= 0 ? 1 : -1;
                   rigidBody2D.velocity = new Vector2(knockbackDirection * knockback,knockback/2);
                }
            }
            else
            {
                Death();
            }
        }
    }

    IEnumerator cooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isInCooldown = false;
    }

    private void Jump()
    {
        rigidBody2D.AddForce(Vector2.up * jumpForce);
    }

    private void DeathOnFall()
    {
        if (transform.position.y < -10f)
        {
            transform.position = respawnpoint;
            Hit(0, null);
        }
    }

    public void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x > 0)
            direction = Vector3.right;
        else
            direction = Vector3.left;
        GameObject bullet =
            Instantiate(bulletPrefab,
            transform.position + direction * 0.8f,
            Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        if(!isInCooldown){
        rigidBody2D.velocity = new Vector2(horizontal, rigidBody2D.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Tilemap") isGrounded = true;
        if(collider.name == "PointA" || collider.name == PointB){
        GameObject warp = collider.transform.parent.gameObject;
        warpA= warp.Find("PointA").gameObject;
        warpB= warp.Find("PointB").gameObject;

    }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Tilemap") isGrounded = false;
    }
}
