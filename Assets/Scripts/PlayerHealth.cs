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

    [Header("Invincibilité")]
    [SerializeField] private float invincibilityTime = 2f;
    [SerializeField] private float blinkSpeed = 0.1f;

    [Header("UI")]
    [SerializeField] private UIManager uiManager;

    private bool isInvincible = false;
    private SpriteRenderer[] spriteRenderers;
    private bool isShielded = false;

    [SerializeField] private GameOverManager gameOverManager;


    void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;

        currentCheckpoint = levelSpawn;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        uiManager.UpdateLives(currentLives);
        uiManager.UpdateHealth(currentHealth);
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage appelé. Shield = " + isShielded);

        if (isShielded) return;
        if (isInvincible) return;

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

    void LoseLife()
    {
        currentLives--;

        uiManager.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            RespawnAtCheckpoint();
        }
    }

    void RespawnAtCheckpoint()
    {
        ResetAllGhosts();

        transform.position = currentCheckpoint.position;

        uiManager.UpdateHealth(currentHealth);

        StartCoroutine(Invincibility());
    }

    void RestartLevelFromSpawn()
    {
        ResetAllGhosts();

        currentLives = maxLives;
        currentHealth = maxHealth;

        currentCheckpoint = levelSpawn;
        transform.position = levelSpawn.position;

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
            sr.enabled = visible;
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

    void ResetAllGhosts()
    {
        GhostAI[] ghosts = FindObjectsByType<GhostAI>(FindObjectsSortMode.None);

        foreach (GhostAI ghost in ghosts)
        {
            ghost.ResetGhost();
        }
    }

    public void FallDamage()
    {
        currentHealth--;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        uiManager.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            LoseLife();
        }
        else
        {
            RespawnAtCheckpoint();
        }
    }
}