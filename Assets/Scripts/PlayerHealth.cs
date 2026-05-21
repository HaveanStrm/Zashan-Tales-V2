using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Points de vie")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Nombre de vies")]
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    [Header("Checkpoints")]
    [SerializeField] private Transform levelSpawn;
    private Transform currentCheckpoint;

    [Header("Ennemis")]
    [SerializeField] private GameObject enemiesParent;

    [Header("Invincibilité")]
    [SerializeField] private float invincibilityTime = 2f;
    [SerializeField] private float blinkSpeed = 0.1f;

    [Header("UI")]
    [SerializeField] private UIManager uiManager;

    [Header("Game Over")]
    [SerializeField] private GameOverManager gameOverManager;

    private bool isInvincible = false;
    private bool isShielded = false;
    private bool isRespawning = false;

    private SpriteRenderer[] spriteRenderers;

    void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;

        currentCheckpoint = levelSpawn;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        uiManager.UpdateHealth(currentHealth);
        uiManager.UpdateLives(currentLives);
    }

    public void TakeDamage(int damage)
    {
        if (isShielded) return;
        if (isInvincible) return;
        if (isRespawning) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        uiManager.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            LoseLife();
            return;
        }

        StartCoroutine(Invincibility());
    }

    public void FallDamage()
    {
        if (isRespawning) return;

        isRespawning = true;

        currentHealth--;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        uiManager.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            LoseLife();
        }
        else
        {
            RespawnAtCheckpoint(false);
        }

        Invoke(nameof(EndRespawn), 0.3f);
    }

    void EndRespawn()
    {
        isRespawning = false;
    }

    void LoseLife()
    {
        currentLives--;
        uiManager.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver();
            }

            return;
        }

        RespawnAtCheckpoint(true);
    }

    void RespawnAtCheckpoint(bool refillHealth)
    {
        ResetAllGhosts();

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
        else if (levelSpawn != null)
        {
            transform.position = levelSpawn.position;
        }

        if (refillHealth)
        {
            currentHealth = maxHealth;
        }

        uiManager.UpdateHealth(currentHealth);

        StartCoroutine(Invincibility());
    }

    void RestartLevelFromSpawn()
    {
        ResetAllGhosts();

        currentLives = maxLives;
        currentHealth = maxHealth;

        currentCheckpoint = levelSpawn;

        if (levelSpawn != null)
        {
            transform.position = levelSpawn.position;
        }

        uiManager.UpdateLives(currentLives);
        uiManager.UpdateHealth(currentHealth);
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void SetShielded(bool value)
    {
        isShielded = value;
    }

    void ResetAllGhosts()
    {
        if (enemiesParent == null) return;

        foreach (Transform enemy in enemiesParent.transform)
        {
            enemy.gameObject.SetActive(true);

            GhostAI ghostAI = enemy.GetComponent<GhostAI>();
            if (ghostAI != null)
            {
                ghostAI.ResetGhost();
            }

            GhostHealth ghostHealth = enemy.GetComponent<GhostHealth>();
            if (ghostHealth != null)
            {
                ghostHealth.ResetHealth();
            }
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibilityTime)
        {
            SetPlayerVisible(false);
            yield return new WaitForSeconds(blinkSpeed);

            SetPlayerVisible(true);
            yield return new WaitForSeconds(blinkSpeed);

            timer += blinkSpeed * 2f;
        }

        SetPlayerVisible(true);
        isInvincible = false;
    }

    void SetPlayerVisible(bool visible)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                sr.enabled = visible;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}