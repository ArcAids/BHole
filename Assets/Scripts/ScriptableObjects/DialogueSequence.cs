using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSequence : ScriptableObject
{
    [SerializeField] public List<Dialogue> dialogues;
    [SerializeField] public bool autoScroll;
    [SerializeField] public bool pauseGame;
    public void Play()
    {
        DialoguePrinter.Instance.StartDialogue(this);
    }
}

[System.Serializable]
public class Dialogue
{
    public string characterName;
    public List<string> lines;
}