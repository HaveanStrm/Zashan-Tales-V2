using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 24f;
    [SerializeField] private float lifeTime = 1.5f;

    private int direction = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection;

        if (direction < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GhostHealth ghost = other.GetComponent<GhostHealth>();

        if (ghost != null)
        {
            ghost.TakeDamage(1);

            Destroy(gameObject);
        }
    }
}