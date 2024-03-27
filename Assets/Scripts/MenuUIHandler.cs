using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI bestScoreTMP;
    public TMP_InputField  playerNameTMP;

    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("START MenuUIHandler");
        
        // Initialize Best Score
        MainManager.LoadData();
        bestScoreTMP.text  = "Best Score: " + MainManager.bestPlayer + ": " + MainManager.bestScore;
        playerNameTMP.text = MainManager.bestPlayer;

        //Initialize Player Name (if it comes back from Main Scene)
        // if(string.IsNullOrEmpty(MainManager.currentPlayer))
        // {
        //     playerNameTMP.text = "";
        // }
        // else
        // {
        //     playerNameTMP.text = MainManager.currentPlayer;
        // }
    }


     public void StartNew()
    {
        Debug.Log("playerName = " + playerNameTMP.text);
        MainManager.currentPlayer = playerNameTMP.text;
        
        SceneManager.LoadScene(1);

    }//StartNew


    public void Exit()
    {
        // MainManager.Instance.SaveData();
        MainManager.SaveData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }//Exit()

    
}
