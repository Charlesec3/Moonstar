using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Button lastCheckpoint;

    void OnEnable()
    {
        lastCheckpoint.Select();
    }
}
