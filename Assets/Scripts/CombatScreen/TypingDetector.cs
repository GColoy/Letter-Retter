using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// Source of raw keystrokes. Subclass to read input from somewhere
    /// other than the keyboard (gamepad, network, scripted test input).
    /// Read by WordDetector, never by CombatCoordinator directly.
    /// </summary>
    abstract class TypingDetector : MonoBehaviour
    {
        /// <summary>
        /// Return characters typed since the last call, then clear the
        /// internal buffer. Must be drain-on-read: a second call with no
        /// new input returns the empty string. Backspace should be
        /// emitted as '\b' or (char)127 — WordDetector handles it.
        /// </summary>
        abstract public string get_latest_keys();
    }
}