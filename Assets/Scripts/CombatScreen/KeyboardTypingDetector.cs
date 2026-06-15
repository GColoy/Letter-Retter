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
            // Backspace is handled explicitly in Update via backspaceKey, because
            // in WebGL the browser consumes the Backspace key and it never arrives
            // here. Ignore it here so desktop (where it does arrive) doesn't delete
            // twice.
            if (c == '\b' || c == (char)127) return;
            buffer.Append(c);
        }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb != null && kb.backspaceKey.wasPressedThisFrame)
                buffer.Append('\b');
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
