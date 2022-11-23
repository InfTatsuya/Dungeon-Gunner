using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [HideInInspector] public bool isDamageable = true;
    [HideInInspector] public Enemy enemy;

    [Tooltip("Populated with the HealthBar component on the HealthBar GameObject")]
    [SerializeField] private HealthBar healthBar;

    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;
    private Player player;
    private Coroutine immunityCoroutine;
    private bool isImmunityAfterHit = false;
    private float immunityTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private WaitForSeconds waitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    private const float spriteFlashInterval = 0.2f;

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        CallHealthEvent(0);

        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        if(player != null)
        {
            if (player.playerDetails.isImmunityAfterHit)
            {
                isImmunityAfterHit = true;
                immunityTime = player.playerDetails.hitImmunityTime;
                spriteRenderer = player.spriteRenderer;
            }
        }
        else if(enemy != null)
        {
            if (enemy.enemyDetails.isImmunityAfterHit)
            {
                isImmunityAfterHit = true;
                immunityTime = enemy.enemyDetails.hitImmunityTime;
                spriteRenderer = enemy.spriteRendererArray[0];
            }
        }

        if(enemy != null && enemy.enemyDetails.isHealthBarDisplayed && healthBar != null)
        {
            healthBar.EnableHealthBar();
        }
        else if(healthBar != null)
        {
            healthBar.DisableHealthBar();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        bool isRolling = false;

        if(player != null)
        {
            isRolling = player.playerControl.isPlayerRolling;
        }

        if (isDamageable && !isRolling)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            PostHitImmunity();

            if(healthBar != null)
            {
                healthBar.SetHealthBarValue((float)currentHealth / startingHealth);
            }
        }
    }

    private void PostHitImmunity()
    {
        if (!gameObject.activeSelf) return;

        if (isImmunityAfterHit)
        {
            if(immunityCoroutine != null)
            {
                StopCoroutine(immunityCoroutine);
            }

            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRenderer));
        }
    }

    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt((immunityTime / spriteFlashInterval) / 2f);

        isDamageable = false;

        while(iterations > 0)
        {
            spriteRenderer.color = Color.red;
            yield return waitForSecondsSpriteFlashInterval;

            spriteRenderer.color = Color.white;
            yield return waitForSecondsSpriteFlashInterval;

            iterations--;
            yield return null;
        }

        isDamageable = true;
    }

    private void CallHealthEvent(int damageAmount)
    {
        healthEvent.CallHealthChangedEvent((float)currentHealth / startingHealth, currentHealth, damageAmount);
    }

    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }

    public void AddHealth(int healthPercent)
    {
        int healthIncrease = Mathf.RoundToInt(startingHealth * healthPercent / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if(totalHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        else
        {
            currentHealth = totalHealth;
        }

        CallHealthEvent(0);
    }
}
