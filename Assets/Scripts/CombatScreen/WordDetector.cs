using System.Text;
using UnityEngine;

namespace CombatScreen
{
    class WordDetector : MonoBehaviour
    {

        [SerializeField] TypingDetector typingDetector;
        readonly StringBuilder current_word = new();

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

        public void new_word()
        {
            current_word.Clear();
        }
    }
}