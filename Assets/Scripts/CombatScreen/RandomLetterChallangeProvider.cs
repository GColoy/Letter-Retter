using UnityEngine;

namespace CombatScreen
{
    // [CreateAssetMenu(
    //     fileName = "RandomLetterKeysProvider",
    //     menuName = "Letter Retter/Keys Provider/Random Letter")]
    // class RandomLetterKeysProvider : ChallengeProviderAsset
    class RandomLetterChallengeProvider : ChallengeProvider
    {
        [SerializeField] private string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public override string getNextChallenge()
        {
            int i = Random.Range(0, alphabet.Length);
            return alphabet[i].ToString();
        }
    }
}
