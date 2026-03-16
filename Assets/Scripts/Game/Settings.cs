using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    [SerializeField] Slider damageSlider;
    [SerializeField] Slider healthSlider;

    [SerializeField] AudioMixer damageMixer;
    [SerializeField] AudioMixer healthMixer;


    public void changeDamageAudio()
    {
        damageMixer.SetFloat("masterVolume", damageSlider.value);
    }

     public void changeHealthAudio()
    {
        healthMixer.SetFloat("masterVolume", healthSlider.value);
    }
}
