using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General")]

    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuScript menuController;
    
    [Header("Mouse")]

    [SerializeField] private TMP_Text SensTextValue;
    [SerializeField] private Slider sensSlider = null;

    [SerializeField] private int InvertMouse = 0;
    [SerializeField] private Toggle invertMouseToggle = null;

    [Header("Audio")]

    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;


    [Header("Enemy AI")]

    [SerializeField] private int AIDifficulty;
    [SerializeField] private TMP_Dropdown AIDifficultyDropdown;

    private void Awake()
    {
        if (canUse)
        {
            if(PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                float volumeNumber = localVolume * 100;
                
                volumeTextValue.text = volumeNumber.ToString("000");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("AIDifficulty"))
            {
                int localDifficulty = PlayerPrefs.GetInt("AIDifficulty");
                AIDifficultyDropdown.value = localDifficulty;
            }
            else
            {
                menuController.ResetButton("General");
            }

            if (PlayerPrefs.HasKey("masterInvertMouse"))
            {
                InvertMouse = PlayerPrefs.GetInt("masterInvertMouse");
                if (InvertMouse == 1)
                {
                    invertMouseToggle.isOn = true;
                } 
                else
                {
                    invertMouseToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("masterSens"))
            {
                float sens = PlayerPrefs.GetFloat("masterSens");
                int sensTextValue = Mathf.RoundToInt(sens);
                SensTextValue.text = sensTextValue.ToString("00");
                sensSlider.value = sens;
            }
            else
            {
                float sens = 2f;
                int sensTextValue = Mathf.RoundToInt(sens);
                SensTextValue.text = sensTextValue.ToString("00");
                sensSlider.value = sens;
            }

        }
    }

}
