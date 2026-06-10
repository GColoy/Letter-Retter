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
    class TimeCombatCoordinator : MonoBehaviour
    {
        /// <summary>Supplies the string the player must type next.</summary>
        [SerializeField] TimedChallengeProvider challengeProvider;
        /// <summary>Tracks the player's cumulative typed string for the current challenge.</summary>
        [SerializeField] WordDetector wordDetector;
        /// <summary>Renders the goal and the player's progress on screen.</summary>
        [SerializeField] TypingDisplay typingDisplay;
        [SerializeField] TimeScoreDisplay scoreDisplay;
        [SerializeField] float WinScore = 10;
        [SerializeField] float CurrentScore = 0;
        [SerializeField] float StepSize = 1;
        [SerializeField] float MaxTime = 20;
        [SerializeField] float HeadStart = 4;
    
        string goal = "";
        TimeSpan allowedTime = new TimeSpan();
        TimeSpan spentTime = new TimeSpan();
        TimeSpan totalTimeSpent = new TimeSpan();

        void Start()
        {
            updateGoal();
        }

        private void updateGoal()
        {
            challengeProvider.genereateNextChallenge();
            goal = challengeProvider.getCurrentGoal();
            allowedTime = challengeProvider.getCurrentTimeSpan();
            spentTime = new TimeSpan();

            typingDisplay.initializeText(goal);
            wordDetector.new_word();
        }
        
        void Update()
        {
            spentTime = spentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
            totalTimeSpent = totalTimeSpent.Add(TimeSpan.FromSeconds(Time.deltaTime));

            string typed = wordDetector.get_current_word();
            typingDisplay.displayProgress(typed);
            scoreDisplay.displayScore(WinScore, CurrentScore, StepSize, (float)allowedTime.TotalSeconds, (float)(allowedTime.TotalSeconds - spentTime.TotalSeconds), (float)((totalTimeSpent.TotalSeconds - HeadStart)/MaxTime));

            if ((totalTimeSpent.TotalSeconds - HeadStart) / MaxTime > CurrentScore / WinScore)
            {
                Debug.Log("You LOST!!!");
                totalTimeSpent = new TimeSpan();
                CurrentScore = 0;
                updateGoal();
                return;
            }

            if (typed == goal)
            {
                CurrentScore += (float)(StepSize * Math.Clamp((allowedTime.TotalSeconds - spentTime.TotalSeconds) / allowedTime.TotalSeconds, 0, 1));
                if (CurrentScore > WinScore)
                {
                    Debug.Log("You WON!!!");
                    totalTimeSpent = new TimeSpan();
                    CurrentScore = 0;
                }
                updateGoal();
            }
        }
    }

}
