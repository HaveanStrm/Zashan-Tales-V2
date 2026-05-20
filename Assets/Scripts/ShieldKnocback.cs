using UnityEngine;

public class ShieldKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 8f;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GhostAI ghost = other.GetComponent<GhostAI>();

        if (ghost != null)
        {
            Vector2 direction = other.transform.position - transform.position;
            ghost.Knockback(direction, knockbackForce);
        }
    }
}