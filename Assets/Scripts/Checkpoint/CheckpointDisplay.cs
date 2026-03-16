using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointDisplay : MonoBehaviour
{
    public static CheckpointDisplay instance;

    TextMeshProUGUI textDisplay;
    [SerializeField] float displayTime;


    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        textDisplay = this.transform.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator displayCheckpoint()
    {
        textDisplay.enabled = true;

        this.transform.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(displayTime);

        textDisplay.enabled = false;
    }
}
