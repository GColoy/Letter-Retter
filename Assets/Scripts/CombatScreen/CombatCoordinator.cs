using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// Drives the combat-screen loop: fetch a challenge, watch the player
    /// type it, advance to the next one when they get it right.
    ///
    /// Hold the only gameplay logic in the folder — scoring, win/lose,
    /// timers, combos all belong here. See README.md for the full picture
    /// and how the three serialized references plug together.
    /// </summary>
    class CombatCoordinator : MonoBehaviour
    {
        /// <summary>Supplies the string the player must type next.</summary>
        [SerializeField] ChallengeProvider challengeProvider;
        /// <summary>Tracks the player's cumulative typed string for the current challenge.</summary>
        [SerializeField] WordDetector wordDetector;
        /// <summary>Renders the goal and the player's progress on screen.</summary>
        [SerializeField] TypingDisplay typingDisplay;
    
        string goal = "";

        void Start()
        {
            goal = challengeProvider.getNextChallenge();
            typingDisplay.initializeText(goal);
        }
        void Update()
        {
            string typed = wordDetector.get_current_word();
            typingDisplay.displayProgress(typed);
            if (typed == goal)
            {
                goal = challengeProvider.getNextChallenge();
                typingDisplay.initializeText(goal);
                wordDetector.new_word();
            }
        }
    }
}
