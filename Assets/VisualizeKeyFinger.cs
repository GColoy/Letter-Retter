using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using UnityEngine.InputSystem; 

public class VisualizeKeyFinger : MonoBehaviour
{
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

    //registers pressed keys and calls UpdateAppearance to change their looks and the corresponding finger
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
}