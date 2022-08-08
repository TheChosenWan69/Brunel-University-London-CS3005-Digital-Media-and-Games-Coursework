using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    // Divide by 10 due to there being 10 health sprites in the health bar system originally.
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth /10;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth /10 ;
    }
}