using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyurikenManager : MonoBehaviour
{
    [SerializeField] AudioClip throwSe;
    [SerializeField] AudioClip stabbedSe;

    Transform tf;
    Rigidbody2D rb;
    Animator animator;
    AudioSource[] syurikenAudio = new AudioSource[2];
    AudioSource throwAudio;
    AudioSource stabbedAudio;
    Renderer syurikenRenderer;
    CircleCollider2D circleCollider2D;

    const float speed = 10.0f;

    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        syurikenAudio = GetComponents<AudioSource>();
        throwAudio = syurikenAudio[0];
        stabbedAudio = syurikenAudio[1];
        syurikenRenderer = GetComponent<Renderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();

        rb.velocity = new Vector2(speed * tf.localScale.x, 0f);
        throwAudio.PlayOneShot(throwSe);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stage"))
        {
            rb.velocity = Vector2.zero;
            animator.speed = 0f;
            Destroy(this.gameObject, 10.0f);
        }
        else if (other.CompareTag("Enemy"))
        {
            stabbedAudio.PlayOneShot(stabbedSe);
            syurikenRenderer.enabled = false;
            Destroy(circleCollider2D);

            Destroy(gameObject, 1.0f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Death") || other.CompareTag("Stage"))
        {
            Destroy(this.gameObject);
        }
    }
}
