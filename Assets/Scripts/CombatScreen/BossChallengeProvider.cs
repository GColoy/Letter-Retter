using UnityEngine;

namespace CombatScreen
{
    /// <summary>
    /// Final-boss challenge source. Serves a curated sequence of themed
    /// opening sentences once, in order, then keeps the fight going with
    /// random picks from a repeatable fallback pool. Content is loaded from
    /// Assets/Resources/bossText.json (kept separate from the dialogue
    /// text.json) using the same Resources.Load idiom as the dialogue loader.
    /// </summary>
    class BossChallengeProvider : ChallengeProvider
    {
        [SerializeField] string resourceName = "bossText";

        [System.Serializable]
        class BossText
        {
            public string[] bossOpening;
            public string[] bossFallback;
        }

        BossText data;
        int openingIndex = 0;

        void load()
        {
            if (data != null) return;

            TextAsset ta = Resources.Load<TextAsset>(resourceName);
            if (ta != null)
            {
                data = JsonUtility.FromJson<BossText>(ta.text);
            }
            // Never hand back null arrays so getNextChallenge can stay simple.
            data ??= new BossText();
            data.bossOpening ??= new string[0];
            data.bossFallback ??= new string[0];
        }

        public override string getNextChallenge()
        {
            load();

            if (openingIndex < data.bossOpening.Length)
            {
                return data.bossOpening[openingIndex++];
            }

            if (data.bossFallback.Length > 0)
            {
                return data.bossFallback[Random.Range(0, data.bossFallback.Length)];
            }

            // No content at all: return something non-empty so the loop survives.
            return data.bossOpening.Length > 0
                ? data.bossOpening[data.bossOpening.Length - 1]
                : "...";
        }
    }
}
