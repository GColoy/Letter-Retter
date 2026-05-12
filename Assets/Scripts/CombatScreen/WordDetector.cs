using UnityEngine;

namespace CombatScreen
{
    class WordDetector : MonoBehaviour
    {

        [SerializeField] TypingDetector typingDetector;
        string current_word = "";

        public string get_current_word()
        {
            current_word += typingDetector.get_latest_keys();
            return current_word;
        }

        public void new_word()
        {
            current_word = "";
        }
    }
}