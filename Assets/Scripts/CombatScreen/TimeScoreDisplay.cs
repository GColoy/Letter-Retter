using UnityEngine;

namespace CombatScreen
{
    public class TimeScoreDisplay : MonoBehaviour
    {
        [SerializeField] Transform DoneProgress;
        [SerializeField] Transform PossibleProgress;
        [SerializeField] Transform FailedProgress;
        /// <summary>
        /// Draws the three bars from already-normalized [0, 1] ratios:
        /// <paramref name="doneProgress"/> is the locked-in progress,
        /// <paramref name="possibleStep"/> is what the current word would add
        /// on top of it, and <paramref name="failedProgress"/> is the chaser.
        /// </summary>
        internal void displayScore(float doneProgress, float possibleStep, float failedProgress)
        {
            setPosition(DoneProgress, Mathf.Clamp01(doneProgress));
            setPosition(PossibleProgress, Mathf.Clamp01(doneProgress + possibleStep));
            setPosition(FailedProgress, Mathf.Clamp01(failedProgress));
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
