using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using UnityEngine.InputSystem; 


/// <summary>
///Example Usage in other class: (The Keyboard must be dragged onto the corresponding field in the inspector)
///public VisualizeKeyFinger visualizer;
///void StarteTutorial()
///{
///  List<string> keys = new List<string> { "F", "J" };
///  visualizer.ShowKeys(keys);
///}
///void BeendeTutorial()
///{
///  visualizer.HideOverlay();
///}
/// </summary>


public class VisualizeKeyFinger : MonoBehaviour
{
    public Dialogue dialogueBox; 
    public GameObject overlay;
    [Header("Hand Settings")]
    public Image handLeft; 
    public Image handRight;
    public Sprite handIdleLeft; 
    public Sprite handIdleRight;

    //List to add each character of the alphabet with corresponding sprites
    [System.Serializable] 
    public struct KeyFingerMapping
    {
        public string key;
        public Image keyImage; 
        public Sprite handSprite;
        public Sprite keySprite; 
        public Sprite keyPressedSprite; 
        public bool isLeftHand; 
    }

    [Header("Key Mappings")]
    public List<KeyFingerMapping> keyMappings; 

    void Start()
    {
        string[] text = {
            "Hallo", 
            "Test"
        };
        dialogueBox.Show(text); 
    }
//Can be used for testing. When pressing a key the corresponding key will be highlighted 
/*
    void Update()
    {
        if (Keyboard.current == null) return;
        foreach (var mapping in keyMappings)
        {
             var control = Keyboard.current[mapping.key.ToLower()] as UnityEngine.InputSystem.Controls.KeyControl;
            if (control != null)
            {
                if (control.wasPressedThisFrame)
                {
                    UpdateAppearance(mapping, true);
                }
                if (control.wasReleasedThisFrame)
                {
                    UpdateAppearance(mapping, false);
                }
            }
        }
    } 
*/

    //Hides the overlay
    public void HideOverlay()
    {
        if (overlay != null)
        {
            overlay.SetActive(false);
        }
    }

    //Resets every key and hand to its default sprite
    private void ResetAllAppearances()
    {
        foreach (var mapping in keyMappings)
        {
            UpdateAppearance(mapping, false);
        }
    }

    //changes the key and hand appearance
    void UpdateAppearance(KeyFingerMapping mapping, bool isPressed)
    {
        if (mapping.keyImage != null)
        {
            mapping.keyImage.sprite = isPressed ? mapping.keyPressedSprite : mapping.keySprite;
        }

        Image targetHand = mapping.isLeftHand ? handLeft : handRight;
        Sprite idleSprite = mapping.isLeftHand ? handIdleLeft : handIdleRight;

        if (targetHand != null)
        {
            targetHand.sprite = isPressed ? mapping.handSprite : idleSprite;
        }
    }

    //activates the overlay and highlights the keys specified by that parameter
    public void ShowKeys(List<string> keysToShow)
    {
        if (overlay != null)
        {
            overlay.SetActive(true);
        }

        //Reset all keys
        ResetAllAppearances();

        foreach (string keyName in keysToShow)
        {
            KeyFingerMapping mapping = keyMappings.Find(m => m.key.Equals(keyName, System.StringComparison.OrdinalIgnoreCase));
            if (mapping.keyImage != null || mapping.handSprite != null)
            {
                UpdateAppearance(mapping, true);
            }
        }
    }
}