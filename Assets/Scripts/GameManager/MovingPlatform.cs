using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private Transform aPoint;
    [SerializeField] private Transform bPoint;

    [Header("Platform Speed")]
    [SerializeField] private float speed;

    private Vector3 target;

    void Start()
    {
        transform.position = aPoint.position;
        target = bPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, aPoint.position) < 0.1f)
        {
            target = bPoint.position;
        }else if (Vector2.Distance(transform.position, bPoint.position) < 0.1f)
        {
            target = aPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}