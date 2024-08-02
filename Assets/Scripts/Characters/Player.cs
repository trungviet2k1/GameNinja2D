using System.Collections;
using UnityEngine;

public class Player : Character
{
    [Header("Character parameter settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Player Attack")]
    [SerializeField] private Transform throwHolder;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float throwDelay;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontal;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDeath = false;
    private int coin = 0;

    void Awake()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
    }

    void Update()
    {
        //Move character with keyboard
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttacking || IsDead) return;

        if (isGrounded)
        {
            if (isJumping) return;

            // Jumping with keyboard Space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (horizontal != 0)
            {
                ChangeAnimation("Run");
            }
            else
            {
                ChangeAnimation("Idle");
            }

            // Attack with keyboard
            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }

            // Throw with keyboard
            if (Input.GetKeyDown(KeyCode.V))
            {
                Throw();
            }
        }

        // Falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            isJumping = false;
            ChangeAnimation("Fall");
        }

        // Moving
        if (horizontal != 0)
        {
            MoveCharacter(horizontal);
        }
        else if (isGrounded && rb.velocity.x != 0)
        {
            StopCharacter();
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        rb = GetComponent<Rigidbody2D>();
        isAttacking = false;

        transform.position = PlayerRespawn.Instance.GetSavePoint();
        ChangeAnimation("Idle");
        DeActiveAttack();

        PlayerRespawn.Instance.SavePoint(transform.position);
        CoinManager.Instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    private void MoveCharacter(float direction)
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        transform.rotation = Quaternion.Euler(new Vector3(0, direction > 0 ? 0 : 180, 0));
    }

    public void Jump()
    {
        if (isGrounded && !isJumping)
        {
            isJumping = true;
            ChangeAnimation("Jump");
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse); 
        }
    }

    private void StopCharacter()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            ChangeAnimation("Attack");
            Invoke(nameof(ResetAttack), 0.5f);
            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f); 
        }
    }

    public void Throw()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            ChangeAnimation("Throw");
            Invoke(nameof(ResetAttack), 0.5f);
            StartCoroutine(ThrowKunai()); 
        }
    }

    private IEnumerator ThrowKunai()
    {
        yield return new WaitForSeconds(throwDelay);
        Instantiate(kunaiPrefab, throwHolder.position, throwHolder.rotation);
    }

    private void ResetAttack()
    {
        isAttacking = false;
        ChangeAnimation("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coin++;
            PlayerPrefs.SetInt("Coin", coin);
            CoinManager.Instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DeathZone"))
        {
            if (!isDeath)
            {
                ChangeAnimation("Die");
                StartCoroutine(Respawn());
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        OnInit();
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
}