using System;
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
    ///
    /// Everything the loop reasons about is kept as a normalized [0, 1]
    /// ratio: <see cref="score"/> is progress toward the win, and the time
    /// ratios are recomputed once at the top of <see cref="Update"/> so the
    /// rest of the frame never has to divide by a duration again.
    /// </summary>
    class TimeCombatCoordinator : DelayedcombatCoordinator
    {
        /// <summary>Supplies the string the player must type next.</summary>
        [SerializeField] TimedChallengeProvider challengeProvider;
        /// <summary>Tracks the player's cumulative typed string for the current challenge.</summary>
        [SerializeField] WordDetector wordDetector;
        /// <summary>Renders the goal and the player's progress on screen.</summary>
        [SerializeField] TypingDisplay typingDisplay;
        [SerializeField] ScoreDisplay scoreDisplay;
        /// <summary>Fraction of the bar gained for one word typed with full time to spare.</summary>
        [SerializeField, Range(0, 1)] float ScorePerWord = 0.1f;
        [SerializeField] float MaxTime = 20;
        [SerializeField] float HeadStart = 4;

        string goal = "";
        float score = 0f;                          // progress toward the win, normalized to [0, 1]
        TimeSpan allowedTime = new TimeSpan();
        TimeSpan spentTime = new TimeSpan();
        TimeSpan totalTimeSpent = new TimeSpan();

        private void updateGoal()
        {
            challengeProvider.genereateNextChallenge();
            goal = challengeProvider.getCurrentGoal();
            allowedTime = challengeProvider.getCurrentTimeSpan();
            spentTime = new TimeSpan();

            typingDisplay.initializeText(goal);
            wordDetector.new_word();
            typingDisplay.displayProgress("");
        }

        // Called by the base when a round (re)starts: wipe the run-long state
        // and hand the player their first goal. activateCombat() runs this
        // before the loop goes live, so there is no separate Start() to seed it.
        public override void resetCombat()
        {
            totalTimeSpent = new TimeSpan();
            score = 0f;
            scoreDisplay.displayScore(0, 1f, 1f);
            updateGoal();
        }

        // Polled by the base while the round is armed: the clock stays paused
        // until the player has typed at least one character of the goal.
        protected override bool hasStartedTyping()
        {
            return !string.IsNullOrEmpty(wordDetector.get_current_word());
        }

        // Driven by the base's Update() only while combat is active.
        public override void updateCombat()
        {
            spentTime = spentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
            totalTimeSpent = totalTimeSpent.Add(TimeSpan.FromSeconds(Time.deltaTime));

            // Boil the durations down to normalized ratios once, up front, so
            // nothing below this line has to think in seconds again.
            float timeLeftRatio = Mathf.Clamp01(1f - (float)(spentTime.TotalSeconds / allowedTime.TotalSeconds));
            float failedProgress = Mathf.Clamp01((float)((totalTimeSpent.TotalSeconds - HeadStart) / MaxTime));
            float possibleStep = ScorePerWord * timeLeftRatio;

            string typed = wordDetector.get_current_word();
            typingDisplay.displayProgress(typed);
            scoreDisplay.displayScore(score, possibleStep, failedProgress);

            if (failedProgress > score)
            {
                combatLost();
                return;
            }

            if (typed == goal)
            {
                score += possibleStep;
                if (score > 1f)
                {
                    combatWon();
                    return;
                }
                updateGoal();
            }
        }
    }

}
