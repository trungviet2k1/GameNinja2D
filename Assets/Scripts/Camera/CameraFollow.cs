using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;

    private Transform target;

    void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}