using UnityEngine;

namespace CombatScreen
{
    abstract class DelayedcombatCoordinator : MonoBehaviour, IAfterDialogAction
    {
        public Dialogue dialogue;
        private bool combatActive = false;

        public void activateCombat()
        {
            resetCombat();
            combatActive = true;
        }

        public abstract void updateCombat();
        public abstract void resetCombat();

        void Start()
        {
            string[] text = {"Lets start the combat", "It starts after you click"};
            dialogue.Show(text, this);
        }

        void Update()
        {
            if (combatActive) updateCombat();
        }

        protected void combatLost()
        {
            resetCombat();
            combatActive = false;
            string[] text = {"Sorry You Lost", "Lets try again", "It starts after you click"};
            dialogue.Show(text, this);
        }

        protected void combatWon()
        {
            resetCombat();
            combatActive = false;
            string[] text = {"Congratulations You Won", "Lets train some more", "It starts after you click"};
            dialogue.Show(text, this);
        }

        public void action()
        {
            activateCombat();
        }
    }
}