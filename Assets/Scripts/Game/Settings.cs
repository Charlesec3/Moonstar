using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider damageSlider;
    [SerializeField] TextMeshProUGUI damageValueDispalyText;

    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthValueDispalyText;

    [SerializeField] AudioMixer damageMixer;
    [SerializeField] AudioMixer healthMixer;


   
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/DamageVolume.txt") == true)
        {
            float dmgVol = float.Parse(File.ReadAllText(Application.persistentDataPath + "/DamageVolume.txt"));
            damageSlider.value = dmgVol;
        }
        else
        {
            damageSlider.value = 0;
        }

        if (File.Exists(Application.persistentDataPath + "/HealthVolume.txt") == true)
        {
            float healthVol = float.Parse(File.ReadAllText(Application.persistentDataPath + "/HealthVolume.txt"));
            healthSlider.value = healthVol;
        }
        else
        {
            healthSlider.value = 0;
        }


        damageMixer.SetFloat("masterVolume", damageSlider.value);
        healthMixer.SetFloat("masterVolume", healthSlider.value);
    }


    public void changeDamageAudio()
    {
        damageMixer.SetFloat("masterVolume", damageSlider.value);
        File.WriteAllText(Application.persistentDataPath + "/DamageVolume.txt",damageSlider.value.ToString());

        damageValueDispalyText.text =  Mathf.RoundToInt(damageSlider.value).ToString();
    }

     public void changeHealthAudio()
    {
        healthMixer.SetFloat("masterVolume", healthSlider.value);
        File.WriteAllText(Application.persistentDataPath + "/HealthVolume.txt", healthSlider.value.ToString());
    
        healthValueDispalyText.text =  Mathf.RoundToInt(healthSlider.value).ToString();
    }
}
