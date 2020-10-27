using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{

    private void Start()
    {
        GameObject.Find("MusicToggle").GetComponent<Toggle>().isOn=MenuScript.music;
        GameObject.Find("SoundToggle").GetComponent<Toggle>().isOn = MenuScript.sound;
        GameObject.Find("DifficultySlider").GetComponent<Slider>().value=MenuScript.difficulty=="HARD"?1:0;
    }
    public void ToggleMusic()
    {
       MenuScript.music = GameObject.Find("MusicToggle").GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("music", MenuScript.music ? 1 : 0);
    }

    public void ToggleSound()
    {
        MenuScript.sound = GameObject.Find("SoundToggle").GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("sound", MenuScript.sound ? 1 : 0);
    }
    public void ChangeDifficulty()
    {
        Slider slider = GameObject.Find("DifficultySlider").GetComponent<Slider>();
        switch (slider.value)
        {
            case 0:
            MenuScript.difficulty= "EASY";
                break;
            case 1:
                MenuScript.difficulty = "HARD";
                break;

        }
        PlayerPrefs.SetString("difficulty", MenuScript.difficulty);
    }
    public void GoBack()
    {
        SceneManager.LoadScene("Menu");
    }
}
