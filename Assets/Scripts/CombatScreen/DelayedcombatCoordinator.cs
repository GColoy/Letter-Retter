using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatScreen
{
    abstract class DelayedcombatCoordinator : MonoBehaviour, IAfterDialogAction
    {
        [SerializeField] public Dialogue dialogue;
        [SerializeField] private string winScene;
        private bool combatActive = false;
        private bool awaitingFirstKey = false;

        public void activateCombat()
        {
            AudioManager.Instance.Play("combat");
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
            string[] text = {"Du musst schneller tippen als dein Gegner (der rote Balken). Denk daran, alle Fehler zu korrigieren, die du gemacht hast.", "Die Zeit laeuft, sobald du anfaengst zu tippen."};
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
            string[] text = {"Du hast leider verloren.", "Versuche es erneut!"};
            dialogue.Show(text, this);
        }

        public void combatWon()
        {
            AudioManager.Instance.Play("main");
            awaitingFirstKey = false;
            combatActive = false;
            resetCombat();
            setCombatInterfaceVisible(false);
            
            string[] text = {"Du hast gewonnen! Der Kampf ist vorbei"};
            dialogue.Show(text, new ActionAfterDialog(() => {SceneManager.LoadScene(winScene);}));
            
            //dialogue.Show(text, this);
        }

        public void action()
        {
            activateCombat();
        }
    }
}