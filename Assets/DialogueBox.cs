using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem; 

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent; 
    public string[] lines; 
    public float textSpeed; 

    private int index; 
    private Coroutine typingCoroutine; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        //if Left Mousebutton is pressed skip to next line or finish the current one 
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (textComponent.text == lines[index])
            {
                NextLine(); 
            }
            else
            {
                StopCoroutine(typingCoroutine);
                textComponent.text = lines[index]; 
            }
        }
    }

    void StartDialogue()
    {
        gameObject.SetActive(true); 
        index = 0; 
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
        }
    }

    //recieve Dialogue and start
    public void RecieveDialogue(string[] text)
    {
        lines = text;
        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true); 
        typingCoroutine = StartCoroutine(TypeLine());
    }
}
