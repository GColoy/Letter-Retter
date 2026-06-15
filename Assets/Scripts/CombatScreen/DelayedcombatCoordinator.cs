using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatScreen
{
    abstract class DelayedcombatCoordinator : MonoBehaviour, IAfterDialogAction
    {
        [SerializeField] public Dialogue dialogue;
        private bool combatActive = false;
        private bool awaitingFirstKey = false;

        public void activateCombat()
        {
            resetCombat();
            // Arm the round but hold the clock: combat only goes live once the
            // player types their first key, reported via hasStartedTyping().
            awaitingFirstKey = true;
            setCombatInterfaceVisible(true);
        }

        public abstract void updateCombat();
        public abstract void resetCombat();
        /// <summary>True once the player has typed at least one character of the current goal.</summary>
        protected abstract bool hasStartedTyping();
        /// <summary>Show or hide the combat interfaces (typing/score displays); shown only between dialogs.</summary>
        protected abstract void setCombatInterfaceVisible(bool visible);

        void Start()
        {
            setCombatInterfaceVisible(false);
            string[] text = {"Lets start the combat. Remember to correct any errors you made!", "You need to outtype your enemy (the red bar)", "It counts after you start typing"};
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
            setCombatInterfaceVisible(false);
            string[] text = {"Sorry You Lost", "Lets try again", "Click for a rematch!"};
            dialogue.Show(text, this);
        }

        protected void combatWon()
        {
            awaitingFirstKey = false;
            combatActive = false;
            resetCombat();
            setCombatInterfaceVisible(false);
            
            string[] text = {"Congratualations you won", "You will be brought back now!"};
            dialogue.Show(text, new ActionAfterDialog(() => {SceneManager.LoadScene("WonFight");}));
            
            //dialogue.Show(text, this);
        }

        public void action()
        {
            activateCombat();
        }
    }
}