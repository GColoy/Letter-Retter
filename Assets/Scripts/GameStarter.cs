using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask clickableLayer = ~0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject start = GameObject.FindWithTag("Enemy");
        if(start != null){
            start.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (cam == null) cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Mausposition -> Welt
        Vector2 world = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // Collider unter Cursor?
        Collider2D col = Physics2D.OverlapPoint(world, clickableLayer);
        if (col != null )
        {
            GameObject start1 = GameObject.FindWithTag("Enemy");
            if(start1 != null){
                start1.GetComponent<SpriteRenderer>().enabled = true;
            }
            if(Mouse.current.leftButton.wasPressedThisFrame){
                SceneManager.LoadScene("StartScene");
            }
        }
        else
        {
            GameObject start2 = GameObject.FindWithTag("Enemy");
            if(start2 != null){
                start2.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
