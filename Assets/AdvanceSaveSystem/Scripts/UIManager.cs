using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //ref to buttons
    public Button resetBtn, retryBtn;
    public Text scoreText, timeText;    //ref to texts
    //ref to ui panels and refab
    public GameObject gameoverPanel, scrollHolder, itemPrefab;
    private List<GameRecord> gameRecords; //list to hold game records

    private float currentTime = 0;  //keep track of total time spend in a game
    private int currentScore = 0;   //keep track of total score achieved in a game

    private bool gameOver = false;  //bool to decide game status
    public bool GameOver { get { return gameOver; } }   //getter fo external scripts
    void Awake()
    {
        //create new list
        gameRecords = new List<GameRecord>();
        if (instance == null) instance = this;
    }

    private void Start()
    {
        if (SaveManager.instance.HasKey<List<GameRecord>>("Records"))
            gameRecords = SaveManager.instance.LoadData<List<GameRecord>>("Records");

        //adding listner to buttons
        resetBtn.onClick.AddListener(() => OnClick(resetBtn));
        retryBtn.onClick.AddListener(() => OnClick(retryBtn));
    }

    //method called when player car collect pickup
    public void IncreaseScore()
    {
        currentScore++; //increase score by 1
        scoreText.text = currentScore.ToString();   //set the score text
    }

    //method call when player car collides with enemy car
    public void GameOverMethod()
    {
        Invoke("InvokeGameOver", 0.5f); //invke InvokeGameOver method after 0.5 sec

        GameRecord newRecord = new GameRecord();
        newRecord.score = currentScore;
        newRecord.time = currentTime;
        gameRecords.Add(newRecord);
        SaveManager.instance.SaveData<List<GameRecord>>("Records", gameRecords);
        SaveManager.instance.CreateData();

        gameOver = true;//set gameover to true
    }

    void InvokeGameOver()
    {
        //spawn the records in the gameover menu
        for (int i = 0; i < gameRecords.Count; i++)
        {
            //create the object
            GameObject listItem = Instantiate(itemPrefab);
            //set its parent
            listItem.transform.SetParent(scrollHolder.transform);
            //set the child Text object text
            listItem.GetComponentInChildren<Text>().text = "No:"  + (i + 1) 
                + " Score:" + gameRecords[i].score 
                + " Time:" + string.Format("{0:0.00}", gameRecords[i].time) + "s";
        }
        gameoverPanel.SetActive(true);//activate the panel
    }

    void Update()
    {
        if (!gameOver)//id gameover is false
        {
            currentTime += Time.deltaTime;//update the time with respect to Time.deltaTime
            //set the time Text
            timeText.text = string.Format("{0:0.00}", currentTime) + "s";
        }
    }

    //assigned to buttons
    void OnClick(Button btn)
    {
        //check for button name
        switch (btn.name)
        {
            case "ResetBtn":
                SaveManager.instance.ClearData();
                break;
            case "RetryBtn":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }
}

//make sure you make the class Serializable so we can serialise it in unity
[System.Serializable]
public class GameRecord
{
    public string name;
    public int score;
    public float time;
}