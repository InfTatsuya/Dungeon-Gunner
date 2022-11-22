using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Space(10)]
    [Header("GameObject References")]

    [Tooltip("The child Bar GameObject")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private GameObject border;

    public void EnableHealthBar()
    {
        border.SetActive(true);
        backgroundImage.SetActive(true);
        healthBar.SetActive(true);
    }

    public void DisableHealthBar()
    {
        border.SetActive(false);
        backgroundImage.SetActive(false);
        healthBar.SetActive(false);
    }

    public void SetHealthBarValue(float healthPercent)
    {
        healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}

