using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sound;
    public AudioClip hittingBase;
    public AudioClip winningSound;
    public AudioClip losingSound;

    private static int maxBaseHP = 500;
    public static int currentBaseHP;
    private static float hpLineRatio;
    public static bool isPlayerDead;
    private GameObject baseLine;
    public static int numOfEnemies;
    private bool gameOver;

    // Start is called before the first frame update
    void Awake()
    {
        music.volume = MenuScript.music == true ? 1 : 0;
        sound.volume = MenuScript.sound == true ? 1 : 0;
        //defind number of enemies based on selected difficulty
        int difficultyMultiplier= MenuScript.difficulty == "EASY" ? 1 : 2;
        numOfEnemies = MenuScript.level * difficultyMultiplier + 5;
    }

    void Start()
    {
        gameOver = false;
        isPlayerDead = false;
        currentBaseHP = 500;
        baseLine = GameObject.Find("baseHPLine");
        hpLineRatio =baseLine.GetComponent<RectTransform>().anchoredPosition.x/maxBaseHP;
        GameObject.Find("numOfEnemies").GetComponent<Text>().text = numOfEnemies.ToString();
        GameObject.Find("CurrentLevel").GetComponent<Text>().text = "LEVEL: " + MenuScript.level;
        GameObject.Find("DifficultyMode").GetComponent<Text>().text = MenuScript.difficulty + " MODE";
        SetBaseHP();
        InvokeRepeating("ScanPath", 0, 1.0f);
    }

    private void ScanPath()
    {
        GameObject.Find("A*").GetComponent<AstarPath>().Scan();
    }

    void Update()
    {
        if (!gameOver)
        {
            if (numOfEnemies <= 0)
            {
                StartCoroutine(Winning());
            }
            if (currentBaseHP <= 0 || isPlayerDead)
            {
                StartCoroutine(Losing());
            }
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Menu");
    }

    private static void SetBaseHP()
    {
        GameObject.Find("BaseHP").GetComponent<Text>().text = currentBaseHP + "/" + maxBaseHP;
        GameObject.Find("baseHPLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(currentBaseHP * hpLineRatio, 0);
        GameObject.Find("baseHPLine").GetComponent<RectTransform>().sizeDelta = new Vector2(currentBaseHP * hpLineRatio*2, 24);
    }

    public void DamageBase(int damage)
    {
        sound.PlayOneShot(hittingBase);
        currentBaseHP = currentBaseHP - damage < 0 ? 0 : currentBaseHP - damage;
        SetBaseHP();
    }

    public static void EnemyKilled()
    {
        numOfEnemies--;
        UpdateData();
    }

    private static void UpdateData() {
        GameObject.Find("numOfEnemies").GetComponent<Text>().text = numOfEnemies.ToString();
        GameObject.Find("BaseHP").GetComponent<Text>().text = currentBaseHP + "/" + maxBaseHP;
        SetBaseHP();
    }

    private void PlayEndSound(AudioClip specificSound)
    {
        gameOver = true;
        music.volume = 0;
        sound.PlayOneShot(specificSound);
    }

    IEnumerator Winning()
    {
        PlayEndSound(winningSound);
        PlayerPrefs.SetInt("Level", MenuScript.level + 1);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    IEnumerator Losing()
    {
        PlayEndSound(losingSound);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Menu");
    }

}
