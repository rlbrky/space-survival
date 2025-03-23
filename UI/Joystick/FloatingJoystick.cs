using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    [SerializeField] public RectTransform knob;
    [HideInInspector] public RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
