using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 

public class VisualizeKeyFinger : MonoBehaviour
{
    [Header("Hand Settings")]
    public Image handLeft; 
    public Image handRight;

    [System.Serializable] 
    public struct KeyFingerMapping
    {
        public string key;
        public Image keyImage; 
        public Sprite handSprite; // Welches Finger-Bild dazu gehört
        public Sprite keySprite; 
        public Sprite keyPressedSprite; 
    }

    [Header("Key Mappings")]
    public List<KeyFingerMapping> keyMappings; 
}