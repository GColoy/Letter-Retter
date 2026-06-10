using UnityEngine;
using UnityEngine.InputSystem;

public class InteractBoxAndDialogue : MonoBehaviour
{
    public GameObject eBox;
    public Dialogue dialogue;
    public InputAction interact;
    private string[] textToShow = {"Test", "Bla"};
    private bool isInRange;
    SceneChanger sceneChanger = new SceneChanger();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eBox.SetActive(false);
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
                dialogue.Show(textToShow, sceneChanger);
            }
        }
    }
}
