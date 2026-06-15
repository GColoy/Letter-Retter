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
        [SerializeField] private int minStringLength = 3;
        [SerializeField] private int maxStringLength = 8;


        public override string getNextChallenge()
        {
            int length = Random.Range(minStringLength, maxStringLength + 1);
            var builder = new System.Text.StringBuilder(length);
            for (int n = 0; n < length; n++)
            {
                int i = Random.Range(0, alphabet.Length);
                builder.Append(alphabet[i]);
            }
            return builder.ToString();
        }
    }
}
