using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Space(10)]
    [Header("GameObject References")]

    [Tooltip("The child Bar GameObject")]
    [SerializeField] private GameObject healthBar;

    public void EnableHealthBar()
    {
        healthBar.SetActive(true);
    }

    public void DisableHealthBar()
    {
        healthBar.SetActive(false);
    }

    public void SetHealthBarValue(float healthPercent)
    {
        healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}

