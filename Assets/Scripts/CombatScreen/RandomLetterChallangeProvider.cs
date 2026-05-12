using UnityEngine;

namespace CombatScreen
{
    // [CreateAssetMenu(
    //     fileName = "RandomLetterKeysProvider",
    //     menuName = "Letter Retter/Keys Provider/Random Letter")]
    // class RandomLetterKeysProvider : ChallangeProviderAsset
    class RandomLetterChallangeProvider : ChallangeProvider
    {
        [SerializeField] private string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public override string getNextChallange()
        {
            int i = Random.Range(0, alphabet.Length);
            return alphabet[i].ToString();
        }
    }
}
