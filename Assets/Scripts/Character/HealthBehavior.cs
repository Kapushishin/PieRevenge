using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour, IDamaging
{
    [SerializeField] private SpriteRenderer _spriteRend;
    private Animator _animator;

    [SerializeField] private float startingHealth;
    public float currentHealth;
    //private bool isDead = false;
    private bool isInvul = false;
    private int playerLayerMask, trapLayerMask;
    [SerializeField] private float framesDuration;
    [SerializeField] private int flashesCount;
    [SerializeField] private GameObject deathParticles;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        currentHealth = startingHealth;
        playerLayerMask = LayerMask.NameToLayer("Player");
        trapLayerMask = LayerMask.NameToLayer("Trap");
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, true);
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
    }

    public void GetDamaged(DamageBehavior enemy, float damage)
    {
        if (!isInvul)
        {
            if (currentHealth > 0)
            {
                currentHealth = currentHealth - damage;
                _animator.SetTrigger("IsHurt");
                StartCoroutine(Invulnerability());
                if (currentHealth == 0)
                {
                    //isDead = true;
                    _animator.SetTrigger("IsDead");
                    Instantiate(deathParticles, transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
