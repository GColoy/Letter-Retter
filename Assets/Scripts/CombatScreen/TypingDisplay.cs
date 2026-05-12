using UnityEngine;

namespace CombatScreen
{
    abstract class TypingDisplay : MonoBehaviour
    {
        abstract public void initializeText(string text);

        abstract public void displayProgress(string text);
    }
}