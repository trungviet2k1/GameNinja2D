using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.HurtSound();
            collision.GetComponent<Character>().OnHit(15);
        }

        if (collision.CompareTag("Enemy"))
        {
            //AudioManager.Instance.HurtSound();
            collision.GetComponent<Character>().OnHit(15);
        }
    }
}