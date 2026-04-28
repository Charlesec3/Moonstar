using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickAudio : MonoBehaviour
{
    Button btn;

    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        if(btn == null)
        {
            btn = this.transform.GetComponentInParent<Button>();

            btn.onClick.AddListener(playAudio);
        }

        if(audioSource == null)
        {
            audioSource = this.gameObject.GetComponent<AudioSource>();
        }        
    }

    void playAudio()
    {
        if(audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
    }
}
