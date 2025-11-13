using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public enum ANIMATION { SLEEPING, WALKING, RUNNING, JUMPING, FALLING }
    ANIMATION animationKind = ANIMATION.SLEEPING;

    [SerializeField] LayerMask stageLayer;
    [SerializeField] GameObject tilemapObj;
    [SerializeField] TileBase woodTile;
    [SerializeField] TileBase itemTile;
    [SerializeField] TileBase emptyTile;
    [SerializeField] GameObject syurikenObj;
    [SerializeField] GameObject fireObj;
    [SerializeField] GameObject fireScrollObj;
    [SerializeField] AudioClip runningSe;
    [SerializeField] AudioClip jumpSe;
    [SerializeField] AudioClip woodBrokenSe;
    [SerializeField] AudioClip mokugyoSe;
    [SerializeField] AudioClip tingSe;
    [SerializeField] AudioClip kotoSe;
    [SerializeField] Camera EditModeCamera;
    [SerializeField] GameObject PanelOfGameOverObj;
    [SerializeField] GameObject PanelOfGameClearObj;

    Transform tf;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;

    AudioSource runningAudio;
    AudioSource jumpAudio;
    AudioSource woodBrokenAudio;
    AudioSource mokugyoAudio;
    AudioSource tingAudio;
    AudioSource kotoAudio;

    Tilemap tilemap;

    const float speed = 6.0f;
    const float jumpPower = 20.0f;
    const float width = 0.425f;
    const float height = 0.925f;

    List<GameObject> attackList = new List<GameObject>();
    List<GameObject> itemList = new List<GameObject>();
    AudioSource[] playerAudios = new AudioSource[3];
    int status = 0;
    GameObject attackObj;
    bool isDead = false;
    string mode;

    Vector3Int originalPositionInt;
    public Vector3Int OriginalPositionInt
    {
        get { return originalPositionInt; }
    }
    bool isDrugged = false;
    public bool IsDrugged
    {
        get { return isDrugged; }
    }

    void Awake()
    {
        tf = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerAudios = GetComponents<AudioSource>();

        runningAudio = playerAudios[0];
        jumpAudio = playerAudios[1];
        woodBrokenAudio = playerAudios[2];
        mokugyoAudio = playerAudios[3];
        tingAudio = playerAudios[4];
        kotoAudio = playerAudios[5];
        runningAudio.clip = runningSe;

        originalPositionInt = new Vector3Int(3, 10, 0);
        tf.position = positionIntToFloat(originalPositionInt);
    }
    Vector3 positionIntToFloat(Vector3Int _position)
    {
        return new Vector3(_position.x + 0.5f, _position.y + height * 0.5f, 0f);
    }

    void Start()
    {
        tilemap = tilemapObj.GetComponent<Tilemap>();

        GameObject dontDestoyObj = GameObject.Find("DontDestroy");
        mode = dontDestoyObj.GetComponent<DontDestroyManager>().Mode;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish") && mode == "Play")
        {
            kotoAudio.PlayOneShot(kotoSe);
            PanelOfGameClearObj.SetActive(true);
        }
        else if (other.CompareTag("FireScroll"))
        {
            Destroy(other.gameObject);
            status = 1;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            PassAway();
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PassAway();
        }
    }
    void PassAway()
    {
        isDead = true;
        rb2d.gravityScale = 0f;
        rb2d.velocity = Vector2.zero;
        capsuleCollider2D.enabled = false;
        runningAudio.Stop();
        mokugyoAudio.PlayOneShot(mokugyoSe);

        animator.SetBool("isStop", false);
        animator.SetBool("isWalk", false);
        animator.SetBool("isDush", false);
        animator.SetInteger("movementY", 0);
        animator.SetTrigger("dead");

        if (mode == "Play") StartCoroutine(DelayFail());
    }
    IEnumerator DelayFail()
    {
        yield return new WaitForSeconds(2.5f);

        tingAudio.PlayOneShot(tingSe);
        PanelOfGameOverObj.SetActive(true);
    }

    public void ResetStatus()
    {
        isDead = false;
        tf.position = positionIntToFloat(originalPositionInt);
        tf.localScale = new Vector3Int(1, 1, 1);
        rb2d.velocity = new Vector2(0f, 0f);
        rb2d.gravityScale = 0f;
        capsuleCollider2D.enabled = true;

        animator.SetBool("isFire", false);
        animator.SetBool("isStop", true);
        animator.SetBool("isWalk", false);
        animator.SetBool("isDush", false);
        animator.SetInteger("movementY", 0);
        animator.Play("ninjaStopAnimation", -1, 0f);
        animationKind = ANIMATION.SLEEPING;

        runningAudio.Stop();

        attackObj = syurikenObj;
        status = 0;

        isDrugged = false;
    }

    public void Activate()
    {
        rb2d.gravityScale = 5f;
    }

    public void PlayerMove()
    {
        if (isDead) return;

        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            tf.localScale = new Vector3Int(1, 1, 1);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb2d.velocity = new Vector2(speed * 1.5f, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
        }
        // 左に移動
        if (Input.GetKey(KeyCode.A))
        {
            tf.localScale = new Vector3Int(-1, 1, 1);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb2d.velocity = new Vector2(-speed * 1.5f, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        || (tf.position.x < 0.4f && Input.GetKey(KeyCode.A))
        || (tf.position.x > 79.6f && Input.GetKey(KeyCode.D)))
        {
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }
    }

    public void PlayerJump()
    {
        if (isDead) return;

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.W) && isGround())
        {
            jumpAudio.PlayOneShot(jumpSe);

            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        Headbutt(-width);
        Headbutt(width);
    }

    bool isGround()
    {
        Vector2 leftStartPoint = (Vector2)tf.position - Vector2.up * height * 0.5f - Vector2.right * width * 0.3f;
        Vector2 rightStartPoint = (Vector2)tf.position - Vector2.up * height * 0.5f + Vector2.right * width * 0.3f;
        Vector2 endPoint = (Vector2)tf.position - Vector2.up * height * 0.6f;

        return Physics2D.Linecast(leftStartPoint, endPoint, stageLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, stageLayer);
    }

    void Headbutt(float _distance)
    {
        Vector3 startPoint = transform.position + Vector3.right * _distance * 0.4f + Vector3.up * height * 0.5f;
        Vector3 endPoint = startPoint + Vector3.up * 0.02f;

        if (Physics2D.Linecast(startPoint, endPoint, stageLayer))
        {
            int x = Mathf.FloorToInt(tf.position.x + _distance * 0.4f);
            int y = Mathf.FloorToInt(tf.position.y);

            var position = new Vector3Int(x, y + 1, 0);

            if (tilemap.GetTile(position) == woodTile)
            {
                woodBrokenAudio.PlayOneShot(woodBrokenSe);

                tilemap.SetTile(position, null);
            }
            else if (tilemap.GetTile(position) == itemTile)
            {
                tilemap.SetTile(position, emptyTile);
                var spawnPosition = new Vector3(x + 0.5f, y + 1.5f, 0f);
                GameObject itemInstance = Instantiate(fireScrollObj, spawnPosition, Quaternion.identity);
                itemList.Add(itemInstance);
            }
        }
    }

    public void PlayerAnimate()
    {
        if (isDead) return;

        if (status == 0)
        {
            animator.SetBool("isFire", false);
        }
        else if (status == 1)
        {
            animator.SetBool("isFire", true);
        }

        if (!isGround())
        {
            animator.SetBool("isStop", false);
            animator.SetBool("isWalk", false);
            animator.SetBool("isDush", false);

            runningAudio.Stop();

            if (rb2d.velocity.y > 0f)
            {
                if (animationKind != ANIMATION.JUMPING)
                {
                    animator.SetInteger("movementY", 1);

                    if (status == 0)
                    {
                        animator.Play("ninjaJumpAnimation", -1, 0f);
                    }
                    else if (status == 1)
                    {
                        animator.Play("fireNinjaJumpAnimation", -1, 0f);
                    }
                    animationKind = ANIMATION.JUMPING;
                }
            }
            else
            {
                if (animationKind != ANIMATION.FALLING)
                {
                    animator.SetInteger("movementY", -1);

                    if (status == 0)
                    {
                        animator.Play("ninjaFallAnimation", -1, 0f);
                    }
                    else if (status == 1)
                    {
                        animator.Play("fireNinjaFallAnimation", -1, 0f);
                    }
                    animationKind = ANIMATION.FALLING;
                }
            }
        }
        else
        {
            animator.SetInteger("movementY", 0);

            if ((Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)
            || !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.LeftShift) && animationKind != ANIMATION.RUNNING)
                {
                    runningAudio.Play();

                    animator.SetBool("isStop", false);
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isDush", true);
                    if (status == 0)
                    {
                        animator.Play("ninjaDushAnimation", -1, 0f);
                    }
                    else if (status == 1)
                    {
                        animator.Play("fireNinjaDushAnimation", -1, 0f);
                    }
                    animationKind = ANIMATION.RUNNING;
                }
                else if (!Input.GetKey(KeyCode.LeftShift) && animationKind != ANIMATION.WALKING)
                {
                    runningAudio.Stop();

                    animator.SetBool("isStop", false);
                    animator.SetBool("isWalk", true);
                    animator.SetBool("isDush", false);
                    if (status == 0)
                    {
                        animator.Play("ninjaWalkAnimation", -1, 0f);
                    }
                    else if (status == 1)
                    {
                        animator.Play("fireNinjaWalkAnimation", -1, 0f);
                    }
                    animationKind = ANIMATION.WALKING;
                }
            }
            else if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)
            || !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            && animationKind != ANIMATION.SLEEPING)
            {
                runningAudio.Stop();

                animator.SetBool("isStop", true);
                animator.SetBool("isWalk", false);
                animator.SetBool("isDush", false);
                if (status == 0)
                {
                    animator.Play("ninjaStopAnimation", -1, 0f);
                }
                else if (status == 1)
                {
                    animator.Play("fireNinjaStopAnimation", -1, 0f);
                }
                animationKind = ANIMATION.SLEEPING;
            }
        }
    }

    public void PlayerAttack()
    {
        if (isDead) return;

        if (status == 0)
        {
            attackObj = syurikenObj;
        }
        else if (status == 1)
        {
            attackObj = fireObj;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var spawnPosition = new Vector3(tf.position.x + width * 0.2f * tf.localScale.x, tf.position.y, 0f);
            GameObject attackInstance = Instantiate(attackObj, spawnPosition, Quaternion.identity);
            Transform attackInstanceTf = attackInstance.GetComponent<Transform>();
            attackInstanceTf.localScale = new Vector3(tf.localScale.x, attackInstanceTf.localScale.y, attackInstanceTf.localScale.z);
            attackList.Add(attackInstance);
        }
    }

    public void AttackReset()
    {
        foreach (GameObject obj in attackList)
        {
            Destroy(obj);
        }
        attackList.Clear();
    }

    public void ItemReset()
    {
        foreach (GameObject obj in itemList)
        {
            Destroy(obj);
        }
        itemList.Clear();
    }

    public void PlayerChangePlace()
    {
        //マウス座標の取得
        Vector3 screenMousePosition = Input.mousePosition;
        //スクリーン座標をワールド座標に変換
        Vector3 worldMousePosition = EditModeCamera.ScreenToWorldPoint(new Vector3(screenMousePosition.x, screenMousePosition.y, 10f));

        if (Input.GetMouseButtonDown(0))
        {
            var worldMousePositionInt = new Vector3Int((int)worldMousePosition.x, (int)worldMousePosition.y, 0);

            if (worldMousePositionInt == originalPositionInt)
            {
                isDrugged = true;

                tf.position = worldMousePosition;
            }
        }

        if (isDrugged)
        {
            tf.position = worldMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrugged = false;

            if (tilemap.HasTile(new Vector3Int((int)tf.position.x, (int)tf.position.y, 0)))
            {
                tf.position = positionIntToFloat(originalPositionInt);
            }
            else
            {
                int x = Mathf.FloorToInt(tf.position.x);
                int y = Mathf.FloorToInt(tf.position.y);

                originalPositionInt = new Vector3Int(x, y, 0);
                tf.position = positionIntToFloat(originalPositionInt);
            }
        }
    }
}
