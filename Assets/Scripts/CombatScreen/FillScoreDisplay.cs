using UnityEngine;
using UnityEngine.UI;

namespace CombatScreen
{
    public class FillScoreDisplay : ScoreDisplay
    {
        [SerializeField] Image DoneProgress;
        [SerializeField] Image PossibleProgress;
        [SerializeField] Image FailedProgress;

        internal override void displayScore(float doneProgress, float possibleStep, float failedProgress)
        {
            DoneProgress.fillAmount = Mathf.Clamp01(doneProgress);
            PossibleProgress.fillAmount = Mathf.Clamp01(doneProgress + possibleStep);
            FailedProgress.fillAmount = Mathf.Clamp01(failedProgress);
        }
    }
}
