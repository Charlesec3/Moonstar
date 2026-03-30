using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject settingsScreen;

    [SerializeField] GameObject keyboardControls;
    [SerializeField] GameObject gamepadControls;

    [SerializeField] Button controlsBtn;
    [SerializeField] Button settingsBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        activateButton();
    }
    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count >= 1)
        {
            gamepadControls.SetActive(true);
            keyboardControls.SetActive(false);
        }
        else if(Gamepad.all.Count == 0)
        {
            gamepadControls.SetActive(false);
            keyboardControls.SetActive(true);
        }
    }

    public void activateButton()
    {
        if(controlsScreen.activeSelf == true)
        {
            settingsBtn.Select();
        }
        else
        {
            controlsBtn.Select();
        }
    }
}
