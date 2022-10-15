using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeachBehavior : MonoBehaviour, IInteracteable
{
    private string _mainFolder = "Food Sprites";
    private Sprite[] _foodSprites;
    public string PromptText => "";

    private void Start()
    {
        LoadSprites();
        var spriteRend = gameObject.GetComponent<SpriteRenderer>();
        spriteRend.sprite = _foodSprites[Random.Range(0, _foodSprites.Length)];
    }

    public bool GetInteracted(InteractionsBehaviour target)
    {
        EventManager.SendPeachUp();
        Destroy(gameObject);
        return true;
    }

    private void LoadSprites()
    {
        _foodSprites = Resources.LoadAll<Sprite>(_mainFolder);
    }
}
