using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePrinter : MonoBehaviour
{
    [SerializeField] DialogueSequence test;
    List<Dialogue> conversation;
    public TMP_Text dialogueBox;
    public TMP_Text characterName;
    public bool autoNext;
    public float speed;
    int currentIndex;
    int dialogueNumber = 0;
    bool isDoneIterating = false;

    public static DialoguePrinter Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        gameObject.SetActive(false);
    }

    // Use this for initialization
    public void StartDialogue(DialogueSequence dialogues)
    {
        gameObject.SetActive(true);
        conversation = dialogues.dialogues;
        autoNext = dialogues.autoScroll;
        Reset();
        characterName.text = conversation[dialogueNumber].characterName;
        Next();
    }
    void Reset()
    {
        StopAllCoroutines();
        isDoneIterating = true;
        currentIndex = -1;
        dialogueNumber = 0;
    }

    [ContextMenu("test")]
    public void TestDialogue()
    {
        StartDialogue(test);
    }

    IEnumerator IterateDialogue(string sentence)
    {
        isDoneIterating = false;
        foreach (var letter in sentence)
        {
            dialogueBox.text += letter;
            yield return new WaitForSeconds(speed);
        }
        isDoneIterating = true;
        if (autoNext)
        {
            yield return new WaitForSeconds(1);
            if(isDoneIterating)
                Next();
        }


    }

    public void Next()
    {
        if (!isDoneIterating)
            return;

        dialogueBox.text = "";
        currentIndex++;
        Debug.Log("clicked next " + conversation.Count);
        if (currentIndex == conversation[dialogueNumber].lines.Count)
        {
            if (!LoadNextDialogue())
            {
                //dialogueBox.text = "Finish.";
                gameObject.SetActive(false);
                isDoneIterating = false;
                return;
            }
            else
            {
                currentIndex = 0;
            }

        }
        StartCoroutine(IterateDialogue(conversation[dialogueNumber].lines[currentIndex]));
    }

    public void Back()
    {
        if (!isDoneIterating)
            return;

        isDoneIterating = false;

        if (currentIndex > 0)
        {
            dialogueBox.text = "";
            currentIndex--;
        }
    }

   bool LoadNextDialogue()
    {
        if (conversation.Count > dialogueNumber+1)
        {
            dialogueNumber++;
            characterName.text = conversation[dialogueNumber].characterName;
            Debug.Log("loaded dialogue" + conversation.Count);
            return true;
        }
        return false;
    }

    public void AutoSwitch()
    {
        autoNext = !autoNext;
    }
}
