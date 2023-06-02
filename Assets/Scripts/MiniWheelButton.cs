using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniWheelButton : MonoBehaviour
{
    public UnityEvent miniButtonPressed;
    
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private KeyCode activationKey, activationKey2;

    private Color baseColor;

    private Color highlightColor;
    // Start is called before the first frame update
    void Start()
    {
        baseColor = sr.material.color;
        highlightColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a + 0.20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(activationKey) || Input.GetKeyDown(activationKey2))
        {
            miniButtonPressed.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        sr.material.color = highlightColor;
    }

    private void OnMouseExit()
    {
        sr.material.color = baseColor;
    }

    private void OnDisable()
    {
        OnMouseExit();
    }

    private void OnMouseDown()
    {
        miniButtonPressed.Invoke();
    }
}
