using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    [Header("Scenes")]
    public string newStandard;
    public string newEndless;

    [Header("Audio Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Mouse Settings")]
    [SerializeField] private TMP_Text mouseSensTextValue = null;
    [SerializeField] private Slider mouseSensSlider = null;
    [SerializeField] private int defaultSens = 2;
    public int mainMouseSens = 2;

    [Header("AI Settings")]
    [SerializeField] private TMP_Dropdown AIDifficultyDropdown;



    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertMouseToggle = null;


    public void NewStandardDialogYes()
    {
        SceneManager.LoadScene(newStandard);
    }

    public void NewEndlessDialogYes()
    {
        SceneManager.LoadScene(newEndless);
    }

    public void ExitButton()
    {
        Application.Quit();
        print("exit");
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        float volumeNumber = volume * 100;
        volumeTextValue.text = volumeNumber.ToString("000");
       
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    public void SetMouseSens(float sensitivity)
    {
        mainMouseSens = Mathf.RoundToInt(sensitivity);
        mouseSensTextValue.text = mainMouseSens.ToString("00");
    }

    public void GameplayApply()
    {
        if (invertMouseToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertMouse", 1);
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertMouse", 0);
        }

        float playerSens = mouseSensSlider.value;
        PlayerPrefs.SetFloat("masterSens", playerSens);

        int selectedDifficulty = AIDifficultyDropdown.value;
        PlayerPrefs.SetInt("AIDifficulty", selectedDifficulty);
        print(selectedDifficulty);
    }


    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            float volumeNumber = defaultVolume * 100;
            volumeTextValue.text = volumeNumber.ToString("000");
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            mouseSensTextValue.text = defaultSens.ToString("00");
            mouseSensSlider.value = defaultSens;
            mainMouseSens = defaultSens;

            AIDifficultyDropdown.value = 0;

            invertMouseToggle.isOn = false;


            GameplayApply();
        }
    }
}
