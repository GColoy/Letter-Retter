using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueBoxTester : MonoBehaviour
{
    public Dialogue dialogueSystem;

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            string[] testText = { 
                "Test-Dialog 1 gestartet!", 
                "Dies ist die zweite Zeile.", 
                "Der Typewriter-Effekt scheint zu funktionieren." 
            };
            dialogueSystem.RecieveDialogue(testText);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            string[] infoText = { 
                "11111111", 
                "22222222", 
                "33333333" 
            };
            dialogueSystem.RecieveDialogue(infoText);
        }
    }
}