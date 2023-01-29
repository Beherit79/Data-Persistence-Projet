using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    
    public TMP_InputField playerNameInput;

    public void StartNewGame()
    {
        Debug.Log("Start New Game "+ playerNameInput.text);
        Debug.Log(playerNameInput.text == "");
        if(playerNameInput.text == "")
        // Alert the user that the name is empty
        {
            Debug.Log("Please enter a name");
            return;
        }
        var player = new Player(playerNameInput.text, 0);
        GameManager.Instance.SetPlayer(player);
        GameManager.SetGameOver(false);
        SceneManager.LoadScene(1);
    }

    public void LoadHighScores()
    {
        SceneManager.LoadScene(2);
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}