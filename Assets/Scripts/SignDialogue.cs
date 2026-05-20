using UnityEngine;

public class SignDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    [TextArea(2, 5)]
    [SerializeField] private string dialogueLine;

    [Header("Interaction")]
    [SerializeField] private GameObject interactPrompt;

    private bool playerInRange = false;

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialogue(dialogueLine);
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.Escape))
        {
            dialogueManager.CloseDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }

            dialogueManager.CloseDialogue();
        }
    }
}