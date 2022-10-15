using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalBackground : MonoBehaviour
{
    [SerializeField] private GameObject _currentBG;
    [SerializeField] private GameObject _newBG;
    private List<SpriteRenderer> _spritesFade = new List<SpriteRenderer>();
    private List<SpriteRenderer> _spritesBright = new List<SpriteRenderer>();
    private bool _activation;
    [SerializeField] private int _power;
    [SerializeField] private GameObject _currentTrigger;
    [SerializeField] private GameObject _newTrigger;

    private void Start()
    {
        _spritesFade = LoadSprites(_currentBG);
        _spritesBright = LoadSprites(_newBG);
    }

    private void Update()
    {
        if (_activation)
        {
            Fading();
            Brightning();
        }
    }

    private List<SpriteRenderer> LoadSprites(GameObject bg)
    {
        int childCount = bg.transform.childCount;
        List<SpriteRenderer> sprites = new List<SpriteRenderer>();

        for (int i = 0; i < childCount; i++)
        {
            Transform child = bg.transform.GetChild(i);
            Transform childChild = child.GetChild(0);
            sprites.Add(child.GetComponent<SpriteRenderer>());
            sprites.Add(childChild.GetComponent<SpriteRenderer>());
        }

        return sprites;
    }

    public void Triggered()
    {
        if (_currentBG.activeSelf)
        {
            _newBG.SetActive(true);

            for (int i = 0; i < _spritesFade.Count; i++)
            {
                _spritesFade[i].color = new Color(_spritesFade[i].color.r, _spritesFade[i].color.g, _spritesFade[i].color.b, 1f);
            }

            for (int i = 0; i < _spritesBright.Count; i++)
            {
                _spritesBright[i].color = new Color(_spritesBright[i].color.r, _spritesBright[i].color.g, _spritesBright[i].color.b, 0f);
            }

            _activation = true;
        }

        _newTrigger.SetActive(true);
    }

    private void Fading()
    {
        for (int i = 0; i < _spritesFade.Count; i++)
        {
            if (_spritesFade[i].color.a > 0)
            {
                float transparency = _spritesFade[i].color.a;
                transparency -= Time.deltaTime * _power;
                _spritesFade[i].color = new Color(_spritesFade[i].color.r, _spritesFade[i].color.g, _spritesFade[i].color.b, transparency);
            }
            else
            {
                _currentTrigger.SetActive(false);
                _currentBG.SetActive(false);
                _activation = false;
            }
        }
    }

    private void Brightning()
    {
        for (int i = 0; i < _spritesBright.Count; i++)
        {
            if (_spritesBright[i].color.a < 1.0f)
            {
                float transparency = _spritesBright[i].color.a;
                transparency += Time.deltaTime * _power;
                _spritesBright[i].color = new Color(_spritesBright[i].color.r, _spritesBright[i].color.g, _spritesBright[i].color.b, transparency);
            }
        }
    }
}
