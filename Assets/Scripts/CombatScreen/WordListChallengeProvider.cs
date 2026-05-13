using System.Collections.Generic;

namespace CombatScreen
{
    class WordListChallengeProvider : ChallengeProvider
    {
        public List<string> ChallengeList = new List<string>();
        private int index = 0;

        public override string getNextChallenge()
        {
            string item = ChallengeList[index];
            index = (index + 1) % ChallengeList.Count;
            return item;
        }
    }
}