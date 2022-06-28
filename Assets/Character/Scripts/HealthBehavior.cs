using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    private SpriteRenderer _spriteRend;
    private Animator _animator;

    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    //private bool isDead = false;
    private bool isInvul = false;
    private int playerLayerMask, trapLayerMask;
    [SerializeField] private float framesDuration;
    [SerializeField] private int flashesCount;
    [SerializeField] private GameObject deathParticles;
    

    private void Awake()
    {
        _spriteRend = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        currentHealth = startingHealth;
        playerLayerMask = LayerMask.NameToLayer("Player");
        trapLayerMask = LayerMask.NameToLayer("Trap");
    }

    public void GetDamaged(float damage)
    {
        if (!isInvul)
        {
            if (currentHealth > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
                Debug.Log(currentHealth.ToString());
                _animator.SetTrigger("IsHurt");
                StartCoroutine(Invulnerability());
                if (currentHealth == 0)
                {
                    //isDead = true;
                    _animator.SetTrigger("IsDead");
                    Instantiate(deathParticles, transform.position, transform.rotation);
                    GetComponent<CharacterControl>().enabled = false;
                    Debug.Log("dead");
                }
            }
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, true);
        Debug.Log("collision ignored");
        isInvul = true;
        for (int i = 0; i < flashesCount; i++)
        {
            _spriteRend.color = Color.red;
            yield return new WaitForSeconds(framesDuration / (flashesCount * 2));
            _spriteRend.color = Color.white;
            yield return new WaitForSeconds(framesDuration / (flashesCount * 2));
        }
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, false);
        isInvul = false;
        Debug.Log("collision turn on");
    }
}
