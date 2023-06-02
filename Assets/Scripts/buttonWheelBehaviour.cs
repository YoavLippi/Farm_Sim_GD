using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class buttonWheelBehaviour : MonoBehaviour
{
    [SerializeField] private KeyCode thisKey;
    
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private UnityEvent onPressed;

    private Color baseColor;

    private Color highlightColor;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        baseColor = sr.material.color;
        highlightColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a + 0.12f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(thisKey))
        {
            onPressed.Invoke();
        }
    }

    private void OnMouseDown()
    {
        onPressed.Invoke();
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
}
