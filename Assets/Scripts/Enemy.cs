using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    public bool indestructible = false;
    public float life = 1f;
    public float knockback = 10f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Hit()
    {
        life -= 1;
        if (!indestructible && life <= 0)
        {
            Destroy (gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            player.Hit(knockback, gameObject);
        }
    }
}
