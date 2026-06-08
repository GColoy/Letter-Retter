using System;
using UnityEngine;

namespace CombatScreen
{
    public class TimeScoreDisplay : MonoBehaviour
    {
        [SerializeField] Transform DoneProgress;
        [SerializeField] Transform PossibleProgress;
        internal void displayScore(float winScore, float currentScore, float stepSize, float maxAddition, float currentAddition)
        {
            float doneProgress = Math.Clamp(currentScore / winScore, 0, 1);
            float possibleProgress = Math.Clamp(doneProgress + (stepSize / winScore) * Math.Clamp(currentAddition / maxAddition, 0, 1), 0, 1);

            setPosition(DoneProgress, doneProgress);
            setPosition(PossibleProgress, possibleProgress);
        }

        [ContextMenu("Preset/Empty")]
        public void PresetEmpty()
        {
            displayScore(100f, 0f, 0.25f, 10, 0);
        }

        [ContextMenu("Preset/Quarter")]
        public void PresetQuarter()
        {
            displayScore(100f, 25f, 0.25f, 10, 5);
        }

        [ContextMenu("Preset/Half")]
        public void PresetHalf()
        {
            displayScore(100f, 50f, 0.25f, 10, 5);
        }

        [ContextMenu("Preset/Three Quarters")]
        public void PresetThreeQuarters()
        {
            displayScore(100f, 75f, 0.25f, 10, 5);
        }

        [ContextMenu("Preset/Full")]
        public void PresetFull()
        {
            displayScore(100f, 100f, 0.25f, 10, 10);
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
