using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// The Combat Coordinator
    /// 
    /// A new word is fetched
    /// Player has to type it
    /// Gets Score on the typing
    /// Win when score is reached
    /// </summary>
    
    class CombatCoordinator : MonoBehaviour
    {
        [SerializeField] ChallangeProvider challangeProvider;
        [SerializeField] WordDetector wordDetector;
        [SerializeField] TypingDisplay typingDisplay;
    
        string goal = "";

        void Start()
        {
            goal = challangeProvider.getNextChallange();
            typingDisplay.initializeText(goal);
        }
        void Update()
        {
            string typed = wordDetector.get_current_word();
            typingDisplay.displayProgress(typed);
            if (typed == goal)
            {
                goal = challangeProvider.getNextChallange();
                typingDisplay.initializeText(goal);
                wordDetector.new_word();
            }
        }
    }
}
