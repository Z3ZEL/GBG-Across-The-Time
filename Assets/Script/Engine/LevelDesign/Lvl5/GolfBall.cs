using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float throwForce = 10f;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Vector2 vector = collision.contacts[collision.contactCount - 1].normal;
            //APPLY THROW FORCE TO BALL
            rb.AddForce(vector * throwForce, ForceMode2D.Impulse);

        }
    }
}
