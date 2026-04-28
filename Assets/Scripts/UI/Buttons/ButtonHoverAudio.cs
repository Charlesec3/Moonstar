using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAudio : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioSource audioSource;

    
    void Start()
    {
        if(audioSource == null)
        {
            audioSource = this.gameObject.GetComponent<AudioSource>();
        }
    }

    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
        
    }
}
