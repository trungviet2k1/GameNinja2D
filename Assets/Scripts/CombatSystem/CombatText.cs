using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    public TextMeshProUGUI hpText;

    public void OnInit(float takeDamage)
    {
        hpText.text = takeDamage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}