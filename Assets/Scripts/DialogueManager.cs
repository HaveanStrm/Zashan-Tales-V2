using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text continueText;

    [Header("Portraits")]
    [SerializeField] private Image playerPortrait;
    [SerializeField] private Image npcPortrait;

    [Header("Effet portrait")]
    [SerializeField] private float activeScale = 1.1f;
    [SerializeField] private float inactiveScale = 0.9f;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(0.35f, 0.35f, 0.35f, 1f);

    [Header("Texte")]
    [SerializeField] private float textSpeed = 0.03f;

    private DialogueLine[] currentLines;
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLines[currentIndex].text;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(string line)
    {
        dialogueActive = true;
        currentIndex = 0;

        DialogueLine simpleLine = new DialogueLine();
        simpleLine.speakerType = SpeakerType.NPC;
        simpleLine.speakerName = "";
        simpleLine.text = line;

        currentLines = new DialogueLine[] { simpleLine };

        dialoguePanel.SetActive(true);

        HidePortraits();

        if (speakerNameText != null)
        {
            speakerNameText.text = "";
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        
        ShowPortraits();

        dialogueActive = true;
        currentLines = lines;
        currentIndex = 0;

        dialoguePanel.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = currentLines[currentIndex];

        if (speakerNameText != null)
        {
            speakerNameText.text = line.speakerName;
        }

        UpdatePortraits(line.speakerType);

        StopAllCoroutines();
        StartCoroutine(TypeLine(line.text));
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            CloseDialogue();
            return;
        }

        ShowLine();
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;
    }

    void UpdatePortraits(SpeakerType speakerType)
    {
        if (playerPortrait == null || npcPortrait == null) return;

        if (speakerType == SpeakerType.Player)
        {
            playerPortrait.transform.localScale = Vector3.one * activeScale;
            npcPortrait.transform.localScale = Vector3.one * inactiveScale;

            playerPortrait.color = activeColor;
            npcPortrait.color = inactiveColor;
        }
        else
        {
            playerPortrait.transform.localScale = Vector3.one * inactiveScale;
            npcPortrait.transform.localScale = Vector3.one * activeScale;

            playerPortrait.color = inactiveColor;
            npcPortrait.color = activeColor;
        }
    }

    void HidePortraits()
    {
        if (playerPortrait != null)
        {
            playerPortrait.gameObject.SetActive(false);
        }

        if (npcPortrait != null)
        {
            npcPortrait.gameObject.SetActive(false);
        }

        if (continueText != null)
        {
            continueText.gameObject.SetActive(false);
        }
    }

    void ShowPortraits()
    {
        if (playerPortrait != null)
        {
            playerPortrait.gameObject.SetActive(true);
        }

        if (npcPortrait != null)
        {
            npcPortrait.gameObject.SetActive(true);
        }

        if (continueText != null)
        {
            continueText.gameObject.SetActive(true);
        }
    }

    public void CloseDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        Time.timeScale = 1f;

        
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    
}