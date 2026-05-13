using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// Source of strings the player must type. Subclass to add a new
    /// challenge generator (random letters, word list, difficulty curve,
    /// network-supplied, etc.) and attach to a GameObject in the scene.
    /// </summary>
    abstract class ChallengeProvider : MonoBehaviour
    {
        /// <summary>
        /// Return the next challenge string. Called once at Start and
        /// again every time the player completes the previous challenge.
        /// Should return a non-empty string.
        /// </summary>
        public abstract string getNextChallenge();
    }
}
