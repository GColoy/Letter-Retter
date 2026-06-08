using System;
using UnityEngine;

namespace CombatScreen
{
    class TimedChallengeProvider : MonoBehaviour
    {
        [SerializeField] ChallengeProvider challengeProvider;

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
            return TimeSpan.FromSeconds(currentGoal.Length * 0.5);
        }
    }
}