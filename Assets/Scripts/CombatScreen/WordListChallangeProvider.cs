using System.Collections.Generic;

namespace CombatScreen
{
    class WordListChallangeProvider : ChallangeProvider
    {
        public List<string> ChallangeList = new List<string>();
        private int index = 0;

        public override string getNextChallange()
        {
            string item = ChallangeList[index];
            index = (index + 1) % ChallangeList.Count;
            return item;
        }
    }
}