using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButton : MonoBehaviour
{
    [SerializeField] Button btn;
    
    void OnEnable()
    {
        btn.Select();
    }
}
