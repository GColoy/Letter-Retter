using UnityEngine;

namespace CombatScreen
{
    abstract class ChallangeProvider : MonoBehaviour
    {
        public abstract string getNextChallange();
    }
}
