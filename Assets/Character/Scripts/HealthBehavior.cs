using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    private SpriteRenderer _spriteRend;
    private Animator _animator;

    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool isDead = false;
    private int playerLayerMask, trapLayerMask;
    [SerializeField] private float framesDuration;
    [SerializeField] private int flashesCount;
    

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
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log(currentHealth.ToString());

        if (currentHealth > 0)
        {
            _animator.SetTrigger("IsHurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!isDead)
            {
                isDead = true;
                //GetComponent<CharacterControl>().enabled = false;
            }
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, true);
        for (int i = 0; i < flashesCount; i++)
        {
            _spriteRend.color = Color.red;
            yield return new WaitForSeconds(framesDuration / (flashesCount * 2));
            _spriteRend.color = Color.white;
            yield return new WaitForSeconds(framesDuration / (flashesCount * 2));
        }
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, false);
    }
}
