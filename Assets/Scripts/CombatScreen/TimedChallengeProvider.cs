using System;
using UnityEngine;

namespace CombatScreen
{
    class TimedChallengeProvider : MonoBehaviour
    {
        [SerializeField] ChallengeProvider challengeProvider;
        [SerializeField] float secondsPerLetter = 0.5f;

        string currentGoal = "";

        internal void genereateNextChallenge()
        {
            currentGoal = challengeProvider.getNextChallenge();
        }

        internal string getCurrentGoal()
        {
            return currentGoal;
        }

        internal TimeSpan getCurrentTimeSpan()
        {
            return TimeSpan.FromSeconds(currentGoal.Length * secondsPerLetter);
        }
    }
}