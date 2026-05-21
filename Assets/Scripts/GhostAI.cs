using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("Joueur")]
    [SerializeField] private Transform player;

    [Header("Détection")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float stopChaseRange = 9f;

    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float returnSpeed = 2f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.3f;
    [SerializeField] private int contactDamage = 1;

    private Rigidbody2D rb;
    private Vector3 spawnPosition;
    private bool isChasing = false;
    private float knockbackTimer = 0f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    void Update()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        if (isChasing && distanceToPlayer >= stopChaseRange)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            FollowPlayer();
        }
        else
        {
            ReturnToSpawn();
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }

    void ReturnToSpawn()
    {
        float distanceToSpawn = Vector2.Distance(transform.position, spawnPosition);

        if (distanceToSpawn <= 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = spawnPosition;
            return;
        }

        Vector2 direction = (spawnPosition - transform.position).normalized;
        rb.linearVelocity = direction * returnSpeed;
    }

    public void Knockback(Vector2 direction, float force)
    {
        knockbackTimer = knockbackDuration;
        rb.linearVelocity = direction.normalized * force;
    }

    public void ResetGhost()
    {
        isChasing = false;
        knockbackTimer = 0f;
        transform.position = spawnPosition;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.ApplyKnockback(transform.position);
        }
    }
}