using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static bool music=true ;
    public static bool sound=true ;
    public static string difficulty="EASY";
    public static int level = 1;

    void Start()
    {
        int savedLvl = PlayerPrefs.GetInt("Level");
        if ( savedLvl== 0)
        {
            level = 1;
            PlayerPrefs.SetInt("Level", 1);
            GameObject.Find("StartText").GetComponent<Text>().text = "START";
            GameObject.Find("LevelText").GetComponent<Text>().text = "Level 1";
        }
        else
        {
            GameObject.Find("StartText").GetComponent<Text>().text = "CONTINUE";
            GameObject.Find("LevelText").GetComponent<Text>().text = "Level "+savedLvl;
            level = savedLvl;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
