using UnityEngine;

namespace CombatScreen
{
    abstract class TypingDetector : MonoBehaviour
    {
        abstract public string get_latest_keys();
    }
}