using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----- Audio Clip -----")]
    [SerializeField] AudioClip background;
    [SerializeField] AudioClip playerHurt;
    [SerializeField] AudioClip coinPickUp;
    [SerializeField] AudioClip swordAttack;
    [SerializeField] AudioClip kunaiThrow;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip jumpSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void HurtSound()
    {
        PlaySFX(playerHurt);
    }

    public void PickUpSound()
    {
        PlaySFX(coinPickUp);
    }

    public void SwordAttackSound()
    {
        PlaySFX(swordAttack);
    }

    public void KunaiThrowSound()
    {
        PlaySFX(kunaiThrow);
    }

    public void DeathSound()
    {
        PlaySFX(deathSound);
    }

    public void JumpSound()
    {
        PlaySFX(jumpSound);
    }
}