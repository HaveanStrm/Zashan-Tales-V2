using System.Collections;
using UnityEngine;

public class ArcaneSystem : MonoBehaviour
{
    public enum ArcaneType
    {
        Sword,
        Staff,
        Cup,
        Coin
    }

    [Header("Arcane actif")]
    [SerializeField] private ArcaneType currentArcane = ArcaneType.Sword;

    [Header("Mana")]
    [SerializeField] public float maxMana = 100f;
    [SerializeField] public float currentMana;

    [Header("Épée / Fireball")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireballCost = 20f;

    [Header("Bâton / Bouclier")]
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float shieldCostPerSecond = 15f;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Coupe / Régénération")]
    [SerializeField] private float manaRegenPerSecond = 25f;

    [Header("Écu / Interaction magique")]
    [SerializeField] private float coinCost = 10f;
    [SerializeField] private float coinDuration = 2f;
    [SerializeField] private float coinRadius = 4f;

    [Header("Effets visuels")]
    [SerializeField] private GameObject CupRegenParticles;
    [SerializeField] private ParticleSystem coinWaveParticles;

    [SerializeField] private UIManager uiManager;

    private bool isShieldActive = false;
    private bool isCoinActive = false;
    private int facingDirection = 1;

    public static ArcaneSystem instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There's more than 1 instance of ArcaneSystem in the scene");
            //Destroy(this);
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentMana = maxMana;

        if (shieldObject != null)
        {
            shieldObject.SetActive(false);
        }

        uiManager.UpdateMana(currentMana, maxMana);
        uiManager.UpdateArcane((int)currentArcane);
    }

    void Update()
    {
        UpdateFacingDirection();
        SwitchArcane();
        UseArcane();
    }

    void UpdateFacingDirection()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            facingDirection = 1;
        }
        else if (moveInput < 0)
        {
            facingDirection = -1;
        }
    }

    void SwitchArcane()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            NextArcane();
        }
        else if (scroll < 0f)
        {
            PreviousArcane();
        }
    }

    void NextArcane()
    {
        int next = (int)currentArcane + 1;

        if (next > 3)
        {
            next = 0;
        }

        currentArcane = (ArcaneType)next;

        uiManager.UpdateArcane((int)currentArcane);

        Debug.Log("Arcane actif : " + currentArcane);
    }

    void PreviousArcane()
    {
        int previous = (int)currentArcane - 1;

        if (previous < 0)
        {
            previous = 3;
        }

        currentArcane = (ArcaneType)previous;

        uiManager.UpdateArcane((int)currentArcane);

        Debug.Log("Arcane actif : " + currentArcane);
    }

    void UseArcane()
    {
        if (currentArcane == ArcaneType.Sword && Input.GetMouseButtonDown(0))
        {
            CastFireball();
        }

        if (currentArcane == ArcaneType.Staff)
        {
            if (Input.GetMouseButton(0))
            {
                UseShield();
            }
            else
            {
                StopShield();
            }
        }

        if (currentArcane == ArcaneType.Cup)
        {
            if (Input.GetMouseButton(0))
            {
                RegenerateMana();
            }
            else
            {
                if (CupRegenParticles != null)
                {
                    CupRegenParticles.SetActive(false);
                }
            }
        }

        if (currentArcane == ArcaneType.Coin && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(UseCoin());
        }
    }

    void CastFireball()
    {
        if (currentMana < fireballCost)
        {
            Debug.Log("Pas assez de mana pour la boule de feu");
            return;
        }

        if (fireballPrefab == null || firePoint == null)
        {
            Debug.LogError("Fireball Prefab ou Fire Point non assigné");
            return;
        }

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Fireball fireballScript = fireball.GetComponent<Fireball>();

        if (fireballScript != null)
        {
            fireballScript.SetDirection(facingDirection);
        }

        currentMana -= fireballCost;
        uiManager.UpdateMana(currentMana, maxMana);

        Debug.Log("Fireball lancée");
    }

    void UseShield()
    {
        if (currentMana <= 0)
        {
            StopShield();
            return;
        }

        if (shieldObject != null && !isShieldActive)
        {
            shieldObject.SetActive(true);
            isShieldActive = true;

            playerHealth.SetShielded(true);
            Debug.Log("Shield ON");
        }

        currentMana -= shieldCostPerSecond * Time.deltaTime;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        uiManager.UpdateMana(currentMana, maxMana);
    }

    void StopShield()
    {
        if (shieldObject != null && isShieldActive)
        {
            shieldObject.SetActive(false);
            isShieldActive = false;

            playerHealth.SetShielded(false);
            Debug.Log("Shield OFF");
        }
    }

    void RegenerateMana()
    {
        currentMana += manaRegenPerSecond * Time.deltaTime;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        uiManager.UpdateMana(currentMana, maxMana);

        if (CupRegenParticles != null)
        {
            CupRegenParticles.SetActive(true);
        }

        Debug.Log("Mana : " + currentMana);
    }

    IEnumerator UseCoin()
    {
        if (isCoinActive) yield break;

        if (currentMana < coinCost)
        {
            Debug.Log("Pas assez de mana pour l'écu");
            yield break;
        }

        currentMana -= coinCost;
        uiManager.UpdateMana(currentMana, maxMana);
        isCoinActive = true;

        if (coinWaveParticles != null)
        {
            coinWaveParticles.Play();
        }

        Debug.Log("Coin actif");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, coinRadius);

        foreach (Collider2D hit in hits)
        {
            InvisiblePlatform invisiblePlatform = hit.GetComponent<InvisiblePlatform>();

            if (invisiblePlatform != null)
            {
                invisiblePlatform.Reveal(coinDuration);
            }
        }

        yield return new WaitForSeconds(coinDuration);

        isCoinActive = false;

        Debug.Log("Coin terminé");
    }

    public bool IsCoinActive()
    {
        return isCoinActive;
    }
}

