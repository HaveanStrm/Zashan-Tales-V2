using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.SetCheckpoint(transform);
                isActivated = true;

                Debug.Log("Checkpoint activé : " + gameObject.name);
            }
        }
    }
}