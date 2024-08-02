using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image imageFill;
    public Vector3 offset;

    private Transform target;
    private float hp;
    private float maxHp;

    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHp, Time.deltaTime * 5f);
        transform.position = target.position + offset;
    }

    public void OnInit(float maxHp, Transform target) 
    {
        this.target = target;
        this.maxHp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}