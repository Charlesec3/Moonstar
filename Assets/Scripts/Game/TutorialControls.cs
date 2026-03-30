using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialControls : MonoBehaviour
{
    [SerializeField] GameObject keyboardContolsDisplay;
    [SerializeField] GameObject gamepadContolsDisplay;

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count >= 1)
        {
            keyboardContolsDisplay.SetActive(false);
            gamepadContolsDisplay.SetActive(true);
        }
        else if(Gamepad.all.Count == 0)
        {
            keyboardContolsDisplay.SetActive(true);
            gamepadContolsDisplay.SetActive(false);
        }
    }
}
