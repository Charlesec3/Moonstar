using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] Button playAgain;

    void OnEnable()
    {
        playAgain.Select();
    }
}
