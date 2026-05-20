using UnityEngine;

public class CameraFollowClamp : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform player;

    [Header("Limites")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float cameraZ;

    void Start()
    {
        cameraZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float clampedX = Mathf.Clamp(player.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(player.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, cameraZ);
    }
}