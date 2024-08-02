using UnityEngine;

public class Kunai : MonoBehaviour
{
    [Header("Weapon Parameters")]
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        OnInit();
    }

    public void OnInit()
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            AudioManager.Instance.HurtSound();
            collision.GetComponent<Character>().OnHit(20);
            OnDespawn();
        }
    }
}