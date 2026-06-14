using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : IAfterDialogAction
{
    public void action()
    {
        SceneManager.LoadScene("CombatScreen");
    }
}
