using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISystem : MonoBehaviour
{
    [SerializeField] private HealthBehavior playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        // Картинка с 10 сердцами в ряд. Закрашивание лишних сердец в зависимости от максимального количества здоровья игрока
        totalHealthBar.fillAmount = playerHealth.StartingHealthPlayer / 10;
    }
    private void Update()
    {
        // Картинка с 10 сердцами в ряд. Закрашивание лишних сердец в зависимости от текущего количества здоровья игрока
        currentHealthBar.fillAmount = playerHealth.CurrentHealthPlayer / 10;
    }
}
