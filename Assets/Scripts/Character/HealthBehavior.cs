using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour, IDamageToPlayer
{
    [SerializeField] private SpriteRenderer _spriteRend;
    private Animator _animator;
    private CharacterSounds _charSounds;

    private bool isInvul = false;
    private int playerLayerMask, trapLayerMask, enemyLayerMask;
    [SerializeField] private float framesDuration;
    [SerializeField] private int flashesCount;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private AudioSource deathSFX;

    [SerializeField] private AudioSource _heartUpSound;

    private SaveLoadSystem _saveLoadSystem;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _charSounds = GetComponent<CharacterSounds>();
        playerLayerMask = LayerMask.NameToLayer("Player");
        trapLayerMask = LayerMask.NameToLayer("Trap");
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        _saveLoadSystem = GameObject.FindGameObjectWithTag("Save System").GetComponent<SaveLoadSystem>();
    }

    private void Start()
    {
        EventManager.OnHeartUp += GetHealed;
    }

    private void OnDisable()
    {
        EventManager.OnHeartUp -= GetHealed;
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
            if (SwitchParametres.HealthCounter > 0)
            {
                SwitchParametres.HealthCounter = SwitchParametres.HealthCounter - damage;
                _animator.SetTrigger("IsHurt");
                StartCoroutine(Invulnerability());
            }

            // Смерть
            if (SwitchParametres.HealthCounter == 0)
            {
                //_animator.SetBool("IsDead", true);
                //_animator.Play("Player1_Death");
                _spriteRend.color = Color.clear;
                Instantiate(deathParticles, transform.position, transform.rotation);
                deathSFX.Play();
                GetComponent<CharacterControl>().CanMove = false;
                Physics2D.IgnoreLayerCollision(playerLayerMask, trapLayerMask, false);
                Physics2D.IgnoreLayerCollision(playerLayerMask, enemyLayerMask, false);
                //_spriteRend.color = new Color(_spriteRend.color.r, _spriteRend.color.g, _spriteRend.color.b, 0.0f);
                Invoke("LoadLastSave", 1.5f);
            }
        }
    }

    private void LoadLastSave()
    {
        GetComponent<CharacterControl>().CanMove = true;
        _saveLoadSystem.LoadGame();
    }

    private void GetHealed()
    {
        _heartUpSound.Play();
        SwitchParametres.HealthCounter++;
    }
}
