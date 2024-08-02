using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;

    [Header("Enemy Attack")]
    [SerializeField] private GameObject attackArea;

    [HideInInspector] public Player target;
    private Rigidbody2D rb;
    private IState currentState;
    private bool isRight = true;

    private void Update()
    {
        if (currentState != null && !IsDead)
        {
            currentState.OnExecute(this); 
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(new IdleState());
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(health.gameObject);
        Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    internal void SetTarget(Player player)
    {
        this.target = player;

        if (target == null)
        {
            ChangeState(new PatrolState());
        }
        else if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    public void Moving()
    {
        ChangeAnimation("Run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnimation("Idle");
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Attack()
    {
        ChangeAnimation("Attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) < attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PatrolPoint"))
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 startPosition = transform.position + (isRight ? Vector3.right : Vector3.left) * attackRange;
        Gizmos.DrawLine(transform.position, startPosition);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
}