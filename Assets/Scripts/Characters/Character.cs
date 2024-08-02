using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Combat System")]
    [SerializeField] protected HealthBar health;
    [SerializeField] protected CombatText damageValue;

    [Header("VFX")]
    [SerializeField] private GameObject hitVFX;

    private float hp;
    private string currentAnimation;

    void Start()
    {
        OnInit();
    }

    public bool IsDead => hp <= 0;

    public virtual void OnInit()
    {
        hp = 100;
        health.OnInit(100, transform);
    }

    public virtual void OnDespawn()
    {
    }

    protected virtual void OnDeath()
    {
        ChangeAnimation("Die");
        AudioManager.Instance.DeathSound();
        Invoke(nameof(OnDespawn), 1f);
    }

    protected void ChangeAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            animator.ResetTrigger(animation);
            currentAnimation = animation;
            animator.SetTrigger(currentAnimation);
        }
    }

    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            ChangeAnimation("TakeDamage");

            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }

            health.SetNewHp(hp);
            GameObject vfx = Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(vfx, 3f);
            Instantiate(damageValue, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }
}