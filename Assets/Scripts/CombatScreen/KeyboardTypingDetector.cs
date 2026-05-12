using System.Text;
using UnityEngine.InputSystem;

namespace CombatScreen
{
    class KeyboardTypingDetector : TypingDetector   
    {
        readonly StringBuilder buffer = new();

        void OnEnable()
        {
            var kb = Keyboard.current;
            if (kb != null) kb.onTextInput += OnTextInput;
        }

        void OnDisable()
        {
            var kb = Keyboard.current;
            if (kb != null) kb.onTextInput -= OnTextInput;
        }

        void OnTextInput(char c)
        {
            buffer.Append(c);
        }

        public override string get_latest_keys()
        {
            if (buffer.Length == 0) return "";
            string s = buffer.ToString();
            buffer.Clear();
            return s;
        }
    }
}
