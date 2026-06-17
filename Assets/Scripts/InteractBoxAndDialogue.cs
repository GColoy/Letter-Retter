using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 

public class InteractBoxAndDialogue : MonoBehaviour
{
    public GameObject eBox;
    public Dialogue dialogue;
    public InputAction interact;
    public GameObject door;
    public VisualizeKeyFinger keyboard;
    public Animator animator;
    //private string[] textToShow = {"Test", "Bla"};
    private bool isInRange;
    SceneChanger sceneChanger = new SceneChanger();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if(enemy != null){
            Debug.Log("Jup");
            enemy.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Debug.Log("Nope");
        }
        eBox.SetActive(false);
        if(door != null)
        {
            door.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(eBox != null)
            {
                isInRange = true;
                eBox.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(eBox != null)
            {
                isInRange = false;
                eBox.SetActive(false);
            }
        }
    }


    void OnEnable() => interact.Enable();
    void OnDisable() => interact.Disable();


    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (interact.ReadValue<float>() > 0f)
            {
                if(eBox != null)
                {
                    eBox.SetActive(false);
                }
                string nameOfChar =  gameObject.name;
                string[] textToShow = null;
                switch (nameOfChar)
                {
                    case "EnterKey":
                        textToShow = LoadDialog("enterkey");
                        dialogue.Show(textToShow, new ActionAfterDialog(() =>
                            {
                            if (door != null) door.SetActive(true);
                            }));
                        break;
                    case "Door":
                        SceneManager.LoadScene("FJFinding");
                        break;
                    case "FJChar":
                        textToShow = LoadDialog("FJplusEnemy1");
                        dialogue.Show(textToShow, new ActionAfterDialog(() =>
                            {
                                List<string> keys = new List<string> { "F", "J" };
                                keyboard.ShowKeys(keys);
                                string[] textToShow2 = LoadDialog("FJplusEnemy2");
                                dialogue.Show(textToShow2, new ActionAfterDialog(() =>
                                    {
                                        keyboard.HideOverlay();
                                        GameObject enemy1 = GameObject.FindWithTag("Enemy");
                                        if(enemy1 != null){
                                            Debug.Log("Jup");
                                            enemy1.GetComponent<SpriteRenderer>().enabled = true;
                                        }
                                        else
                                        {
                                            Debug.Log("Nope");
                                        }
                                        string[] textToShow3 = LoadDialog("FJplusEnemy3");
                                        dialogue.Show(textToShow3, new ActionAfterDialog(() =>
                                            {
                                                List<string> keys2 = new List<string> { "D", "K" };
                                                keyboard.ShowKeys(keys2);
                                                string[] textToShow4 = LoadDialog("FJplusEnemy4");
                                                dialogue.Show(textToShow4, new ActionAfterDialog(() =>
                                                {
                                                    SceneManager.LoadScene("CombatScreen");
                                                }));
                                            }));
                                    }
                                ));
                            }));
                        break;
                    case "FJCharAfterWon":
                        textToShow = LoadDialog("FJAfterWon");
                        dialogue.Show(textToShow, new ActionAfterDialog(() =>
                            {
                                SceneManager.LoadScene("EndScreen");
                            }));
                        break;
                    case "EnterKeyExplainSkip":
                        textToShow = LoadDialog("EnterExplainSkip");
                        dialogue.Show(textToShow, new ActionAfterDialog(() =>
                            {
                                if (door != null) door.SetActive(true);
                            }));
                        break;
                    case "DoorToEndFight":
                        SceneManager.LoadScene("HackerConfrontation");
                        break;
                    case "Hacker":
                        string[] hackerPreLaugth = LoadDialog("HackerPreLaugh");
                        string[] hackerWhileLaught = LoadDialog("HackerWhileLaugh");
                        string[] hackerPostLaught = LoadDialog("HackerPostLaugh");
                        dialogue.Show(hackerPreLaugth, new ActionAfterDialog(() =>
                        {
                            animator.SetBool("laughing", true);
                            dialogue.Show(hackerWhileLaught, new ActionAfterDialog(() =>
                            {
                                animator.SetBool("laughing", false);
                                dialogue.Show(hackerPostLaught, new ActionAfterDialog(() =>
                                {
                                    SceneManager.LoadScene("CombatScreen-Hacker");
                                }));
                            }));
                        }));
                        break;
                    
                    default:
                        textToShow = new string[0];
                        break;
                }
            }
        }
    }
    public static string[] LoadDialog(string key) {
        TextAsset ta = Resources.Load<TextAsset>("text"); // omit .json
        if (ta == null) return null;
        DialogCollection data = JsonUtility.FromJson<DialogCollection>(ta.text);
        switch (key) {
            case "enterkey": return data.enterkey;
            case "enemyone": return data.enemyone;
            case "FJplusEnemy1": return data.FJplusEnemy1;
            case "FJplusEnemy2": return data.FJplusEnemy2;
            case "FJplusEnemy3": return data.FJplusEnemy3;
            case "FJplusEnemy4": return data.FJplusEnemy4;
            case "FJAfterWon": return data.FJAfterWon;
            case "EnterExplainSkip": return data.EnterExplainSkip;
            case "HackerPreLaugh": return data.HackerPreLaugh;
            case "HackerWhileLaugh": return data.HackerWhileLaugh;
            case "HackerPostLaugh": return data.HackerPostLaugh;
            // add more cases as needed
            default: return null;
        }
    }
}

class ActionAfterDialog : IAfterDialogAction
{
    private readonly Action _a;
    public ActionAfterDialog(Action a) => _a = a;
    public void action() => _a();
}
public class DialogCollection {
    public string[] enterkey;
    public string[] FJplusEnemy1;
    public string[] FJplusEnemy2;
    public string[] FJplusEnemy3;
    public string[] FJplusEnemy4;
    public string[] FJAfterWon;
    public string[] enemyone;
    public string[] EnterExplainSkip;
    public string[] HackerPreLaugh;
    public string[] HackerWhileLaugh;
    public string[] HackerPostLaugh;
    // add more fields for other keys
}

