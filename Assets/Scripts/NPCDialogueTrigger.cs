using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueLine[] lines;
    [SerializeField] private bool playOnlyOnce = true;

    private bool hasPlayed = false;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (playOnlyOnce && hasPlayed) return;

        hasPlayed = true;
        dialogueManager.StartDialogue(lines);
    }
}