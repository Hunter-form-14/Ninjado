using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireManager : MonoBehaviour
{
    Transform tf;
    Rigidbody2D rb;

    const float speed = 8.0f;

    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(speed * tf.localScale.x, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Death"))
        {
            Destroy(this.gameObject);
        }
    }
}
