using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Mana")]
    [SerializeField] private Image manaFill;

    [Header("Arcanes")]
    [SerializeField] private Image swordIcon;
    [SerializeField] private Image staffIcon;
    [SerializeField] private Image cupIcon;
    [SerializeField] private Image coinIcon;

    [Header("Points de vie")]
    [SerializeField] private GameObject hp1;
    [SerializeField] private GameObject hp2;
    [SerializeField] private GameObject hp3;

    [Header("Vies")]
    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;

    void Start()
    {

    }

    void Update()
    {

    }

    public void UpdateMana(float currentMana, float maxMana)
    {
        if (manaFill == null) return;

        manaFill.fillAmount = currentMana / maxMana;
    }

    public void UpdateArcane(int arcaneIndex)
    {
        if (arcaneIndex == 0) swordIcon.transform.SetAsLastSibling();
        if (arcaneIndex == 1) staffIcon.transform.SetAsLastSibling();
        if (arcaneIndex == 2) cupIcon.transform.SetAsLastSibling();
        if (arcaneIndex == 3) coinIcon.transform.SetAsLastSibling();
    }

    public void UpdateHealth(int currentHealth)
    {
        hp1.SetActive(currentHealth >= 1);
        hp2.SetActive(currentHealth >= 2);
        hp3.SetActive(currentHealth >= 3);
    }

    public void UpdateLives(int currentLives)
    {
        life1.SetActive(currentLives >= 1);
        life2.SetActive(currentLives >= 2);
        life3.SetActive(currentLives >= 3);
    }
}