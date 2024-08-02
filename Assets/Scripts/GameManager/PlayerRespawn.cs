using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn Instance { get; private set; }

    private Vector3 savePoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePoint(Vector3 position)
    {
        savePoint = position;
    }

    public Vector3 GetSavePoint()
    {
        return savePoint;
    }
}