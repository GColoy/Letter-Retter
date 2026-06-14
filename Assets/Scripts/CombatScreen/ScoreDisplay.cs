using UnityEngine;

namespace CombatScreen
{
    public abstract class ScoreDisplay : MonoBehaviour
    {
        /// <summary>
        /// Draws the three bars from already-normalized [0, 1] ratios:
        /// <paramref name="doneProgress"/> is the locked-in progress,
        /// <paramref name="possibleStep"/> is what the current word would add
        /// on top of it, and <paramref name="failedProgress"/> is the chaser.
        /// </summary>
        internal abstract void displayScore(float doneProgress, float possibleStep, float failedProgress);
    }
}
