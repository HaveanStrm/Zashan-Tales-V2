using UnityEngine;

public enum SpeakerType
{
    Player,
    NPC
}

[System.Serializable]
public class DialogueLine
{
    public SpeakerType speakerType;
    public string speakerName;

    [TextArea(2, 5)]
    public string text;
}