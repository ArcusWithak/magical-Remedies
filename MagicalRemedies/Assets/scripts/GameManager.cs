using System;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    //private ignorethis saveFileSystem;
    private string path;
    public bool saveLoaded = false;

    public int NumberOfMenuLevels;

    public int currentLevelIndex { get; private set; }

    public int currentLevel = 2;

    private Scene scene;

    private int score;

    private bool playTimeRunningOut;

    private GameObject player;

    private GameObject textBox;
    private Text itemText;
    private Text amountText;
    private Text timerText;

    private string[] itemNames;

    #region audio and voicelines
    public AudioSource music;
    public List<AudioSource> levelFinish;
    public List<AudioSource> closeToFinish;
    public List<AudioSource> LevelStart;
    public List<AudioSource> runningOutOfTime;
    public List<AudioSource> damageLines;

    public AudioSource SpeedUp;
    #endregion

    #region LevelTimer - variables
    public float[] loseTimerPerLevel;

    private int levelIndex;

    private float currentTimer = 6000;

    public bool startTimer;
    #endregion

    #region Next level requirement - variables
    public int[] nextLevelRequirement;

    private int amountOfNessasrayItems;
    #endregion

    #region load level event
    //addd listener
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    //remove listener
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    //on Level Load
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //readjust level variables
        this.scene = scene;

        //log level load
        //print("scene has been loaded");
        //print(this.scene.name);

        #region LevelIncrease

        if (this.scene.buildIndex == 3)
        {
            currentLevel++;
        }

        if (this.scene.buildIndex >= NumberOfMenuLevels)
        {
            //play music
            music.Play();

            player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerScript = player.GetComponent<PlayerController>();

            //get UI components
            textBox = player.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
            itemText = textBox.transform.GetChild(0).GetComponent<Text>();
            amountText = textBox.transform.GetChild(1).GetComponent<Text>();
            timerText = textBox.transform.GetChild(2).GetChild(0).GetComponent<Text>();

            //update text UI
            itemText.text = itemNames[currentLevel - 1];
            amountText.text = "Gathered: " + amountOfNessasrayItems + "/" + nextLevelRequirement[currentLevel - 1];

            if (this.scene.buildIndex == NumberOfMenuLevels + 1)
            {
                player.transform.Translate(0, 2, 0);
                player.transform.localScale = new Vector3(2, 2, 2);
                playerScript.stats.SetMovementSpeed(playerScript.movementSpeed * 2);
            }
            else if (this.scene.buildIndex != NumberOfMenuLevels + 2)
            {
                player.transform.Translate(0, currentLevel * 0.1f, 0);
                player.transform.localScale = new Vector3(currentLevel * 0.10f, currentLevel * 0.10f, currentLevel * 0.10f);
                playerScript.stats.SetMovementSpeed(playerScript.movementSpeed * 0.4f);
            }
            else
            {
                playerScript.stats.SetTurnSpeedIncrease(5);
                playerScript.stats.SetMovementSpeed(playerScript.movementSpeed * 10);
            }

            switch (currentLevel)
            {
                case 1:
                    LevelStart[0].Play();
                    break;

                case 6:
                    LevelStart[2].Play();
                    break;

                default:
                    LevelStart[1].Play();
                    break;
            }
        }

        //store current level index
        this.currentLevelIndex = this.scene.buildIndex;

        #endregion

        if (this.scene.buildIndex > NumberOfMenuLevels - 1)
        {
            //start timers
            currentTimer = loseTimerPerLevel[currentLevel - 1];
            startTimer = true;
        }
    }
    #endregion

    #region method

    private void Awake()
    {
        #region Singelton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        #region save
        //finding baseDirectory
        path = AppDomain.CurrentDomain.BaseDirectory;

        //combining path with saveFileName
        path = Path.Combine(path, "SaveSystem.exe");

        if (File.Exists(path))
        {

            Process.Start(path, "reading");

            string value;

            //finding baseDirectory
            path = AppDomain.CurrentDomain.BaseDirectory;

            //combining path with saveFileName
            path = Path.Combine(path, "saveFile.txt");

            if (File.Exists(path))
            {
                value = File.ReadAllText(path);

                if (int.TryParse(value, out currentLevel))
                {
                    currentLevel--;
                    saveLoaded = true;
                }
                UnityEngine.Debug.Log(saveLoaded);
            }
            else
            {
                UnityEngine.Debug.Log(saveLoaded);
            }
        }

        UnityEngine.Debug.Log(saveLoaded);
        #endregion

        #region audioLoad
        Transform audioSource = transform.GetChild(0).GetChild(0);
        //load group of audio
        for (int i = 0; i < audioSource.childCount; i++)
        {
            runningOutOfTime.Add(audioSource.GetChild(i).GetComponent<AudioSource>());
        }

        audioSource = transform.GetChild(0).GetChild(1);
        //load group of audio
        for (int i = 0; i < audioSource.childCount; i++)
        {
            damageLines.Add(audioSource.GetChild(i).GetComponent<AudioSource>());
        }

        audioSource = transform.GetChild(0).GetChild(2);
        //load group of audio
        for (int i = 0; i < audioSource.childCount; i++)
        {
            levelFinish.Add(audioSource.GetChild(i).GetComponent<AudioSource>());
        }

        audioSource = transform.GetChild(0).GetChild(3);
        //load group of audio
        for (int i = 0; i < audioSource.childCount; i++)
        {
            LevelStart.Add(audioSource.GetChild(i).GetComponent<AudioSource>());
        }
        //load speed up audio
        SpeedUp = transform.GetChild(0).GetChild(4).GetComponent<AudioSource>();
        #endregion

        #region assigning item strings
        itemNames = new string[5];

        itemNames[0] = "gather books and scrolls \n to find the recipes";
        itemNames[1] = "gather small plants";
        itemNames[2] = "gather gems and minerals";
        itemNames[3] = "gather giant mushrooms";
        itemNames[4] = "pick up the infected trees";
        #endregion
    }

    private void Update()
    {
        #region Escape button quit
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            string path;

            //finding baseDirectory
            path = AppDomain.CurrentDomain.BaseDirectory;

            //combining path with saveFileName
            path = Path.Combine(path, "SaveSystem.exe");

            if (File.Exists(path))
            {
                Process.Start(path, currentLevel.ToString());
            }

            Application.Quit();
        }
        #endregion

        #region Timer Losing Condition
        levelIndex = currentLevel - 1;

        switch (levelIndex)
        {
            case 0:
                LevelTimerCounter(levelIndex);
                break;
            case 1:
                LevelTimerCounter(levelIndex);
                break;
            case 2:
                LevelTimerCounter(levelIndex);
                break;
            case 3:
                LevelTimerCounter(levelIndex);
                break;
            case 4:
                LevelTimerCounter(levelIndex);
                break;
            case 5:
                LevelTimerCounter(levelIndex);
                break;
        }
        #endregion
    }

    #region LevelTimer - Fuction
    private void LevelTimerCounter(int levelIndex)
    {
        //increase timer
        if (startTimer)
        {
            currentTimer -= Time.deltaTime;
            string seconds;
            if ((int)currentTimer % 60 < 10) { seconds = "0" + (int)currentTimer % 60; } else { seconds = ((int)currentTimer % 60).ToString(); }
            timerText.text = ((int)currentTimer / 60) + ":" + seconds;
        }

        //give warning when running out
        if (currentTimer < loseTimerPerLevel[levelIndex] * 0.15f && !playTimeRunningOut)
        {
            playTimeRunningOut = true;
            runningOutOfTime[UnityEngine.Random.Range(0, runningOutOfTime.Count)].Play();
            StartCoroutine(TimerFlicker());
        }

        //reset level on timer runout
        if (currentTimer <= 0)
        {
            startTimer = false;
            currentTimer = 10;
            playTimeRunningOut = false;
            LoseConditionMet();
        }
    }

    private IEnumerator TimerFlicker()
    {
        do
        {
            yield return new WaitForSeconds(0.5f);

            if (timerText != null)
            {
                timerText.color = new Color(255, 0, 0, 255);
            }

            yield return new WaitForSeconds(0.5f);

            if (timerText != null)
            {
                timerText.color = new Color(228, 186, 107, 255);
            }

        } while (currentTimer < loseTimerPerLevel[levelIndex] * 0.15f);
    }

    #endregion

    #region LoseCondition - Fuction
    public void LoseConditionMet()
    {
        amountOfNessasrayItems = 0;
        LoadLevel(-1, "DeathScreen");
    }
    #endregion

    #region Get/Set - Score
    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score, bool addScore = false)
    {
        if (addScore)
        {
            this.score += score;
        }
        else
        {
            this.score = score;
        }
    }
    #endregion

    #region increasing nessasry item count
    public void IncreaseItemCount()
    {
        amountOfNessasrayItems++;
        amountText.text = "Gathered: " + amountOfNessasrayItems + "/" + nextLevelRequirement[currentLevel - 1];
        if (amountOfNessasrayItems >= nextLevelRequirement[currentLevel - 1])
        {
            amountOfNessasrayItems = 0;
            levelFinish[UnityEngine.Random.Range(0, levelFinish.Count)].Play();

            //increase level number
            currentLevel++;

            playTimeRunningOut = false;
            startTimer = false;
            currentTimer = Mathf.Infinity;
            timerText = null;
            if (currentLevel != 6)
            {
                LoadLevel(NumberOfMenuLevels - 2);
            }
            else
            {
                LoadLevel(0);
                Destroy(this.gameObject);
            }
        }
    }
    #endregion

    #region Loading new level fuction
    public void LoadLevel(int levelIndex, string levelName = "")
    {
        if (this.scene.buildIndex > NumberOfMenuLevels - 1)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (levelName != "")
        {
            SceneManager.LoadScene("Assets/Scenes/" + levelName + ".unity");
        }
        else
        {
            SceneManager.LoadScene(levelIndex);
        }

    }
    #endregion

    #endregion
}
