using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject deathParticles;

    void Start()
    {

    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}