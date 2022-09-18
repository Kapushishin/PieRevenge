using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    private AudioClip[] _stepGround, _stepWater, _stepWood;
    private AudioClip _stepClip;
    private AudioClip[] _jumpGround, _jumpWater, _jumpWood;
    private AudioClip _jumpClip;
    private AudioClip[] _dashes;
    private AudioClip _dashClip;
    private AudioClip[] _hurts;
    private AudioClip _hurtClip;
    private AudioClip[] _slashes;
    private AudioClip _slashClip;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    private float _lowPitch = 0.7f;

    // чтобы брать из этого скрипта теги поверхностей
    private CharacterControl _charactercontrol;

    // названия папок пути к дорожкам
    private string _mainFolder = "Character", 
        _stepsFolder = "Footsteps", _jumpsFolder = "Jumps", _dashesFolder = "Dashes", _hurtesFolder = "Hurt",
        _slashesFolder = "Sword Slashes",
        _groundFolder = "Ground", _waterFolder = "Water", _woodFolder = "Wood";

    private void Start()
    {
        _charactercontrol = GetComponent<CharacterControl>();
        // загружаем дорожки
        LoadSounds();
    }

    // добавляем из конкретных папок дорожки в массив, можно свободно менять содержимое папок 
    private void LoadSounds()
    {
        _stepGround = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _stepsFolder + "/" + _groundFolder);
        _stepWater = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _stepsFolder + "/" + _waterFolder);
        _stepWood = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _stepsFolder + "/" + _woodFolder);

        _jumpGround = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _jumpsFolder + "/" + _groundFolder);
        _jumpWater = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _jumpsFolder + "/" + _waterFolder);
        _jumpWood = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _jumpsFolder + "/" + _woodFolder);

        _dashes = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _dashesFolder);

        _hurts = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _hurtesFolder);

        _slashes = Resources.LoadAll<AudioClip>(_mainFolder + "/" + _slashesFolder);
    }

    // звук шагов, вызывается ивентом в анимационном клипе Player1_Run
    private void PlayStepsSound()
    {
        // в зависимости от тэга поверхности, на которой стоит персонаж (тэг берется из скрипта CharacterControl)
        switch (_charactercontrol.tagSurface)
        {
            // если ***, то в поле клипа добавить рандомную дорожку из массива папки соответствующей названию тега
            case "Ground":
                _stepClip = _stepGround[UnityEngine.Random.Range(0, _stepGround.Length)];
                break;
            case "Water":
                _stepClip = _stepWater[UnityEngine.Random.Range(0, _stepWater.Length)];
                break;
            case "Wood":
                _stepClip = _stepWood[UnityEngine.Random.Range(0, _stepWood.Length)];
                break;
        }
        // рандомно менять уровень громкости в заданном диапазоне 
        _audioSource.pitch = UnityEngine.Random.Range(_lowPitch, 1f);
        // проиграть дорожку 1 раз
        _audioSource.PlayOneShot(_stepClip);
    }

    // звук прыжка, вызывается при вхождении в колизию поверхности
    private void PlayJumpSound()
    {
        // в зависимости от тэга поверхности, на которой стоит персонаж (тэг берется из скрипта CharacterControl)
        switch (_charactercontrol.tagSurface)
        {
            // если ***, то в поле клипа добавить дорожку из папки соответствующей названию тега
            case "Ground":
                _jumpClip = _jumpGround[0];
                break;
            case "Water":
                _jumpClip = _jumpWater[0];
                break;
            case "Wood":
                _jumpClip = _jumpWood[0];
                break;
        }
        // рандомно менять уровень громкости в заданном диапазоне 
        _audioSource.pitch = UnityEngine.Random.Range(_lowPitch, 1f);
        // проиграть дорожку 1 раз
        _audioSource.PlayOneShot(_jumpClip);
        // так как в этом скрипте есть обработчик вхождения в коллизию земли, проиграть анимацию приземления
        _animator.Play("Player1_Landing");
    }

    // звук дэша, вызывается из скрипта CharacterControl
    public void PlayDashSound()
    {
        _dashClip = _dashes[0];
        // проиграть дорожку 1 раз
        _audioSource.PlayOneShot(_dashClip);
    }

    // звук получения урона, вызывается ивентом в анимационном клипе Player1_Hurt
    private void PlayHurtSound()
    {
        _hurtClip = _hurts[0];
        // проиграть дорожку 1 раз
        _audioSource.PlayOneShot(_hurtClip);
    }

    private void PlaySlashesSound()
    {
        _slashClip = _slashes[UnityEngine.Random.Range(0, _slashes.Length)];
        _audioSource.pitch = UnityEngine.Random.Range(_lowPitch, 1f);
        _audioSource.PlayOneShot(_slashClip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // здесь вызывается звук прыжка
        PlayJumpSound();
    }
}
