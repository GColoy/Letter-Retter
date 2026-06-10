using System;
using UnityEngine;

namespace CombatScreen
{
    public class TimeScoreDisplay : MonoBehaviour
    {
        [SerializeField] Transform DoneProgress;
        [SerializeField] Transform PossibleProgress;
        [SerializeField] Transform FailedProgress;
        internal void displayScore(float winScore, float currentScore, float stepSize, float maxAddition, float currentAddition, float failedScore)
        {
            float doneProgress = Math.Clamp(currentScore / winScore, 0, 1);
            float possibleProgress = Math.Clamp(doneProgress + (stepSize / winScore) * Math.Clamp(currentAddition / maxAddition, 0, 1), 0, 1);

            setPosition(DoneProgress, doneProgress);
            setPosition(PossibleProgress, possibleProgress);
            setPosition(FailedProgress, Math.Clamp(failedScore, 0, 1));
        }

        private void setPosition(Transform progressbar,float progress)
        {
            Vector3 doneScale = progressbar.localScale;
            doneScale.x = progress;
            progressbar.localScale = doneScale;
            Vector3 donePosition = progressbar.localPosition;
            donePosition.x = (float)((progress * 0.5) - 0.5);
            progressbar.localPosition = donePosition;
        }

    }
}
