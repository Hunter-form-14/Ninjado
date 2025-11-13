using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] LayerMask stageLayer;
    [SerializeField] Sprite ghostDeadSprite;

    Transform tf;
    Rigidbody2D rb;
    Renderer ghostRenderer;
    CircleCollider2D circleCollider2D;
    SpriteRenderer spriteRenderer;
    Animator animator;

    const float speed = 3.0f;
    const float width = 0.775f;
    const float height = 0.75f;
    const float fadeSpeed = 1.0f;

    bool isFacingRight = false;
    bool isDeath = false;

    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        ghostRenderer = GetComponent<Renderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            isDeath = true;
            Destroy(circleCollider2D);
            spriteRenderer.sprite = ghostDeadSprite;
            Destroy(animator);

            StartCoroutine(FadeOut(ghostRenderer));
        }
    }
    IEnumerator FadeOut(Renderer _renderer)
    {
        // Set initial alpha value
        float alpha = 1.0f;

        // Gradually decrease alpha value over time
        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime * fadeSpeed; // Adjust fadeSpeed according to your preference
            Color newColor = _renderer.material.color;
            newColor.a = alpha;
            _renderer.material.color = newColor;
            yield return null;
        }

        // Once fully transparent, destroy the game object
        Destroy(_renderer.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        GhostMove();
        GhostChangeDirection();
    }

    void GhostMove()
    {
        if (isDeath)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            // 右に移動
            if (isFacingRight)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                tf.localScale = new Vector3Int(1, 1, 1);
            }
            // 左に移動
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                tf.localScale = new Vector3Int(-1, 1, 1);
            }
        }
    }

    void GhostChangeDirection()
    {
        if (isGround()
        && (isOnCliff() || isCollided()))
        {
            isFacingRight = !isFacingRight;
        }
    }
    bool isGround()
    {
        Vector2 leftStartPoint = (Vector2)tf.position - Vector2.up * height * 0.5f - Vector2.right * width * 0.3f;
        Vector2 rightStartPoint = (Vector2)tf.position - Vector2.up * height * 0.5f + Vector2.right * width * 0.3f;
        Vector2 endPoint = (Vector2)tf.position - Vector2.up * height * 0.6f;

        // 左右どちらかのレイが地面に当たっているかを返す
        return Physics2D.Linecast(leftStartPoint, endPoint, stageLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, stageLayer);
    }
    bool isOnCliff()
    {
        Vector2 startPoint = (Vector2)tf.position + Vector2.right * width * 0.5f * tf.localScale.x;
        Vector2 endPoint = startPoint - Vector2.up * height;

        return !Physics2D.Linecast(startPoint, endPoint, stageLayer)
            && tf.position.x > 3.0f
            && tf.position.x < 77.0f;
    }
    bool isCollided()
    {
        Vector2 origin = (Vector2)(tf.position + tf.right * width * 0.5f * tf.localScale.x);
        Vector2 direction = new Vector2(tf.localScale.x, 0f);
        float distance = 0.1f;

        RaycastHit2D hit2D = Physics2D.Raycast(origin, direction, distance);

        // レイキャストの結果を検証
        return hit2D.collider != null
            && hit2D.transform.gameObject != this.gameObject
            && (hit2D.transform.gameObject.CompareTag("Enemy") || hit2D.transform.gameObject.CompareTag("Stage"));
    }
}
