using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Destination du téléporteur")]
    [SerializeField] private Transform destination;

    [Header("Temps avant réutilisation")]
    [SerializeField] private float cooldown = 0.5f;

    private bool canTeleport = true;

    void Start()
    {
        if (destination == null)
        {
            Debug.LogWarning("Destination non assignée dans le Teleporter !");
        }
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si c’est le joueur
        if (!other.CompareTag("Player")) return;

        // Vérifie si le téléporteur est prêt
        if (!canTeleport) return;

        // Récupère le ArcaneSystem du joueur
        ArcaneSystem arcaneSystem = other.GetComponent<ArcaneSystem>();

        // Vérifie si le Coin est actif
        if (arcaneSystem == null || !arcaneSystem.IsCoinActive())
        {
            Debug.Log("Le Coin n'est pas actif !");
            return;
        }

        // Téléportation
        Teleport(other);
    }

    private void Teleport(Collider2D player)
    {
        if (destination == null) return;

        player.transform.position = destination.position;

        canTeleport = false;

        Invoke(nameof(ResetTeleport), cooldown);
    }

    private void ResetTeleport()
    {
        canTeleport = true;
    }
}