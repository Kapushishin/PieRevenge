using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maximumHealth;
    private int damage = 1;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void Update()
    {
        if (currentHealth > maximumHealth)
            currentHealth = maximumHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;   
            else
                hearts[i].sprite = emptyHeart;

            if (i < maximumHealth)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentHealth -= damage;
        }
    }

    public void GetDamaged()
    {
        currentHealth -= damage;
    }
}
