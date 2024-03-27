using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int   LineCount = 6;
    public Rigidbody Ball;

    public Text       BestScoreText;
    public Text       ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int  m_Points;
    
    private bool m_GameOver = false;

    //Data Persistence
    public static MainManager Instance;

    public static string currentPlayer;
    public static string bestPlayer;
    public static int    bestScore;


    private void Awake()
    {
        // Debug.Log("AWAKE MainManager");
            
        // //If an Instance already exists, destory this object to prevent having multiple Main Manager objects in the scene
        // if(Instance != null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        // //the first time it stores the Instance
        // Instance = this;
        // DontDestroyOnLoad(gameObject);

        MainManager.LoadData();

    }//Awake()

    
    // Update the text at the top for the Best Score
    void ShowBestScoreText()
    {
        BestScoreText.text = "Best Score: " + MainManager.bestPlayer + ": " + MainManager.bestScore;
    }

    void UpdateCurrentScore()
    {
        ScoreText.text = "Score for " + currentPlayer + ": " + m_Points;
    }


    
    // Start is called before the first frame update
    void ResetGame()
    {
        m_Points   = 0;
        m_Started  = false;
        m_GameOver = false;
        GameOverText.SetActive(false);

        ShowBestScoreText();
        UpdateCurrentScore();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        Ball.gameObject.SetActive(true);
        Ball.transform.position = new Vector3(0, 1.0f, 0);
    }


    void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //reload the scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        // ScoreText.text = "Score for " + currentPlayer + ": " + m_Points;
        UpdateCurrentScore();
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        
        //check for Best Score
        if(m_Points > MainManager.bestScore)
        {
            MainManager.bestScore  = m_Points;
            MainManager.bestPlayer = MainManager.currentPlayer;
            MainManager.SaveData();    
            
            ShowBestScoreText();
        }
        
    }

    //Data Persistence
        [System.Serializable]
    class PesistentData
    {
        public string BestPlayer;
        public int    BestScore;
        
    }//class PesistentData

    public static void SaveData()
    {
        Debug.Log("Saving Data");

        PesistentData data = new PesistentData();
        data.BestPlayer    = MainManager.bestPlayer;
        data.BestScore     = MainManager.bestScore;

        string json = JsonUtility.ToJson(data);

        //save the data as a local file
        string path = Application.persistentDataPath + "/savefile.json";
        File.WriteAllText(path, json);
    }//SaveColor()

    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(path);

        if(File.Exists(path))
        {
            string        json = File.ReadAllText(path);
            PesistentData data = JsonUtility.FromJson<PesistentData>(json);

            MainManager.bestPlayer = data.BestPlayer;
            MainManager.bestScore  = data.BestScore;

            Debug.Log("bestPlayer = " + bestPlayer);
            Debug.Log("bestScore = " + bestScore);
        }
        else
        {
            MainManager.bestPlayer = "Nobody yet";
            MainManager.bestScore  = 0;
        }
    }//LoadColor()

    

}
