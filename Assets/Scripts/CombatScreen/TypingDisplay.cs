using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// Renders the current challenge and the player's progress against
    /// it. Subclass to change the visual treatment (colours, animation,
    /// 3D letter tiles, etc.). See TMPTypingDisplay for a reference
    /// implementation that uses TextMeshPro rich-text colouring.
    /// </summary>
    abstract class TypingDisplay : MonoBehaviour
    {
        /// <summary>
        /// Called once when a new challenge starts. Cache the goal
        /// string if your subclass needs to compare against it later.
        /// </summary>
        abstract public void initializeText(string text);

        /// <summary>
        /// Called every frame with the player's cumulative typed string
        /// (may be shorter than, equal to, or longer than the goal, and
        /// may contain wrong characters). Render whatever feedback you
        /// want.
        /// </summary>
        abstract public void displayProgress(string text);
    }
}