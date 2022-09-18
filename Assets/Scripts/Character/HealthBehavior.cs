using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour, IDamageToPlayer
{
    [SerializeField] private SpriteRenderer _spriteRend;
    private Animator _animator;
    private CharacterSounds _charSounds;

    [SerializeField] public float StartingHealthPlayer;
    public float CurrentHealthPlayer;
    //private bool isDead = false;
    private bool isInvul = false;
    private int playerLayerMask, trapLayerMask, enemyLayerMask;
    [SerializeField] private float framesDuration;
    [SerializeField] private int flashesCount;
    [SerializeField] private GameObject deathParticles;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _charSounds = GetComponent<CharacterSounds>();

        CurrentHealthPlayer = StartingHealthPlayer;
        playerLayerMask = LayerMask.NameToLayer("Player");
        trapLayerMask = LayerMask.NameToLayer("Trap");
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
    }

    // Неуязвимость на короткое время после получения урона
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, true);
        Physics2D.IgnoreLayerCollision(playerLayerMask, enemyLayerMask, true);
        isInvul = true;
        for (int i = 0; i < flashesCount; i++)
        {
            _spriteRend.color = Color.red;
            yield return new WaitForSeconds(framesDuration / (flashesCount * 2));
            _spriteRend.color = Color.white;
            yield return new WaitForSeconds(framesDuration / (flashesCount * 2));
        }
        Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, false);
        Physics2D.IgnoreLayerCollision(playerLayerMask, enemyLayerMask, false);
        isInvul = false;
    }

    // Реализация интерфейса IDamaging. Поведение при получении урона от врага
    public void GetDamaged(EnemyDamageBehavior enemy, float damage)
    {
        if (!isInvul)
        {
            // Получение урона
            if (CurrentHealthPlayer > 0)
            {
                CurrentHealthPlayer = CurrentHealthPlayer - damage;
                _animator.SetTrigger("IsHurt");
                //_charSounds.PlayHurtSound();
                StartCoroutine(Invulnerability());

                // Смерть
                if (CurrentHealthPlayer == 0)
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
