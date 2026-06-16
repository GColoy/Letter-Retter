using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem;
using System;


/// <summary>
/// Example Usage in other class: 
/// /// First drag the DialogueBoxSystem prefab from the Assets/Prefabs folder into your hierarchie 
/// The DialogueBox (Not the DialogueBoxSystem but the DialogueBox) prefab must be dragged onto the corresponding field in the inspector
/// public Dialogue dialogueBox; 
/// void StartDialogue() 
/// {
///     string[] text = {
///         "Hallo", 
///         "Test"
///     };
///     dialogueBox.Show(text); 
/// }
/// </summary>


public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent; 
    public string[] lines; 
    public float textSpeed; 
    public static event Action OnDialogueStarted; 
    public static event Action OnDialogueEnded; 

    private int index;
    private Coroutine typingCoroutine;
    private IAfterDialogAction afterDialogAction;

    // Update is called once per frame
    void Update()
    {
        //if Left Mousebutton is pressed skip to next line or finish the current one 
        var kb = Keyboard.current; 
        bool advance = Mouse.current.leftButton.wasPressedThisFrame
                || (kb != null && kb.enterKey.wasPressedThisFrame)
                || (kb != null && kb.numpadEnterKey.wasPressedThisFrame)
                || (kb != null && kb.spaceKey.wasPressedThisFrame);

        if (advance)
        {
            if (lines == null || lines.Length == 0) 
            {
                return; 
            }

            if (textComponent.text == lines[index])
            {
                NextLine(); 
            }
            else
            {
                if(typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                textComponent.text = lines[index]; 
            }
        }
    }

    public void Show(string[] text, IAfterDialogAction action)
    {
        Show(text);
        afterDialogAction = action;
    }

    //recieve Dialogue and start
    public void Show(string[] text)
    {
        gameObject.SetActive(true);
        OnDialogueStarted?.Invoke();
        afterDialogAction = null;
        lines = text;
        index = 0;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textComponent.text = string.Empty;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); 
        }
    }

    //start next line
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++; 
            textComponent.text = string.Empty; 
            typingCoroutine = StartCoroutine(TypeLine()); 
        }
        else
        {
            gameObject.SetActive(false);
            OnDialogueEnded?.Invoke();
            afterDialogAction?.action();
        }
    }
}
