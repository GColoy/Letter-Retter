using System.Text;
using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// Sits between a TypingDetector (raw keystrokes) and the
    /// CombatCoordinator. Maintains the cumulative string the player
    /// has typed for the current challenge and applies backspace.
    /// Not abstract — there is only one reasonable implementation.
    /// </summary>
    class WordDetector : MonoBehaviour
    {
        [SerializeField] TypingDetector typingDetector;
        readonly StringBuilder current_word = new();

        /// <summary>Cumulative typed string since the last new_word().</summary>
        public string get_current_word()
        {
            string latest = typingDetector.get_latest_keys();
            if (!string.IsNullOrEmpty(latest))
            {
                foreach (char c in latest)
                {
                    if (c == '\b' || c == (char)127)
                    {
                        if (current_word.Length > 0)
                            current_word.Length -= 1;
                    }
                    else if (!char.IsControl(c))
                    {
                        current_word.Append(c);
                    }
                }
            }
            return current_word.ToString();
        }

        /// <summary>Clear the buffer. Call when a challenge is solved.</summary>
        public void new_word()
        {
            current_word.Clear();
        }
    }
}