using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int MaxHighScores = 5;
    public static GameManager Instance;
    public Text bestScoreText;

    public bool isGameOver;
    private Player _bestPlayer;
    private Player _currentPlayer;
    private List<Player> _players = new List<Player>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("Initializing GameManager");
        _players = LoadScore();
        
        var bestPlayer = Instance.GetBestPlayer();
        bestScoreText.text = "Best Score: " + bestPlayer.Name + " - " + bestPlayer.Score; 
        
        DontDestroyOnLoad(gameObject);
    }


    public static void SetGameOver(bool status)
    {
        Instance.isGameOver = status;
    }

    public void SetCurrentPlayer(Player player)
    {
        _currentPlayer = player;
    }

    public Player GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public Player GetBestPlayer()
    {
        return _bestPlayer;
    }
    
    private void SetBestPlayer(Player player)
    {
        _bestPlayer = player;
    }
    
    public void SaveScore(Player player)
    {
        var saveFilePath = GetSaveFilePath();
        var newPlayer = new Player(player.Name, player.Score);
        Debug.Log(player.Name + " -> " + player.Score);
        var saveData = new SaveData();
        SetHighScore(newPlayer);

        SetBestPlayer(_players[0]);
        foreach (var p in _players)
        {
            Debug.Log("Saving " + p.Name + " -> " + p.Score);
            saveData.highScores.Add(p.SaveToString());
        }

        var json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
    }


    private List<Player> LoadScore()
    {
        var saveFilePath = GetSaveFilePath();

        if (!File.Exists(saveFilePath))
        {
            InitializePlayers();
            return _players;
        }

        var json = File.ReadAllText(saveFilePath);
        var saveData = JsonUtility.FromJson<SaveData>(json);
        // transform json to player
        foreach (var player in saveData.highScores) _players.Add(Player.CreateFromJson(player));

        InitializePlayers();
        SetBestPlayer(_players[0]);

        return _players;
    }

    public List<Player> GetPlayers()
    {
        return _players;
    }

    private void InitializePlayers()
    {
        var nbPlayers = _players.Count;
        for (var i = 0; i < MaxHighScores - nbPlayers; i++)
        {
            Debug.Log("Adding player " + i);
            var player = new Player("Player " + i, 0);
            _players.Add(player);
        }

        _players = _players.OrderByDescending(x => x.Score).ToList();
    }

    private void SetHighScore(Player player)
    {
        Debug.Log("Setting high score for " + player.Name + " -> " + player.Score + " count players" + _players.Count);
        if (_players.Count == 0)
        {
            _players.Add(player);
            Debug.Log("No high scores yet, setting first high score " + _players);
            return;
        }

        for (var i = 0; i < _players.Count; i++)
            if (player.Score > _players[i].Score)
            {
                Debug.Log(player.Name + " - Score:" + player.Score + " is higher than " + _players[i].Name +
                          " - Score:" + _players[i].Score);
                for (var j = _players.Count - 1; j > i; j--) _players[j] = _players[j - 1];

                _players[i] = player;
                break;
            }
    }

    private static string GetSaveFilePath()
    {
        return Application.persistentDataPath + "/savefile.json";
    }

    // SerializedClass SaveData
    [Serializable]
    public class SaveData
    {
        public List<string> highScores = new List<string>();
    }
}