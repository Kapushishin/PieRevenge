using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Animator _animator;

    // названия папок пути к дорожкам
    private string _mainFolder = "Enemies",
        _stepsFolder = "Footsteps", _hurtFolder = "Hurt", _flapsFolder = "Flaps", _slimeFolder = "Slime";

    private float _lowPitch = 0.7f;

    private AudioClip[] _steps;
    private AudioClip _stepClip;
    private AudioClip[] _hurts;
    private AudioClip _hurtClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        LoadSounds();
    }

    private void LoadSounds()
    {
        switch (GetComponent<EnemyBehavior>()._typeOfMonster)
        {
            case (EnemyBehavior.MonsterType.shroom):
                _steps = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _stepsFolder);
                break;
            case (EnemyBehavior.MonsterType.bat):
                _steps = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _flapsFolder);
                break;
            case (EnemyBehavior.MonsterType.slime):
                _steps = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _slimeFolder);
                break;
        }

        _hurts = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _hurtFolder);
    }

    // звук ходьбы
    private void PlayStepsSound()
    {
        _stepClip = _steps[0];
        _audioSource.pitch = Random.Range(_lowPitch, 1f);
        _audioSource.PlayOneShot(_stepClip);
    }

    // звук получения урона, вызывается в скрипте EnemyBehavior
    public void PlayEnemyHurtSound()
    {
        _hurtClip = _hurts[0];
        // проиграть дорожку 1 раз
        _audioSource.pitch = Random.Range(_lowPitch, 1f);
        _audioSource.PlayOneShot(_hurtClip);
    }

}
