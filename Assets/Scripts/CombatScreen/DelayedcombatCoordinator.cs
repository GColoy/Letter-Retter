using UnityEngine;

namespace CombatScreen
{
    abstract class DelayedcombatCoordinator : MonoBehaviour, IAfterDialogAction
    {
        [SerializeField] public Dialogue dialogue;
        [SerializeField] private bool combatActive = false;
        [SerializeField] private bool awaitingFirstKey = false;

        public void activateCombat()
        {
            resetCombat();
            // Arm the round but hold the clock: combat only goes live once the
            // player types their first key, reported via hasStartedTyping().
            awaitingFirstKey = true;
        }

        public abstract void updateCombat();
        public abstract void resetCombat();
        /// <summary>True once the player has typed at least one character of the current goal.</summary>
        protected abstract bool hasStartedTyping();

        void Start()
        {
            string[] text = {"Lets start the combat", "It counts after you start typing"};
            dialogue.Show(text, this);
        }

        void Update()
        {
            if (awaitingFirstKey)
            {
                if (!hasStartedTyping()) return;   // hold until the player starts typing
                awaitingFirstKey = false;
                combatActive = true;
            }
            if (combatActive) updateCombat();
        }

        protected void combatLost()
        {
            awaitingFirstKey = false;
            combatActive = false;
            resetCombat();
            string[] text = {"Sorry You Lost", "Lets try again", "click to start the next round"};
            dialogue.Show(text, this);
        }

        protected void combatWon()
        {
            awaitingFirstKey = false;
            combatActive = false;
            resetCombat();
            string[] text = {"Congratulations You Won", "Lets train some more", "Click to start the next round"};
            dialogue.Show(text, this);
        }

        public void action()
        {
            activateCombat();
        }
    }
}