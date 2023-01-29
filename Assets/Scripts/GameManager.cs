using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameOver;
    private Player _player;
    private List<Player> _players = new List<Player>();
    private const int MaxHighScores = 5;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        _players = LoadScore();
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int points)
    {
        var newScore = _player.Score + points;
        _player.Score = newScore;
    }

    public static void SetGameOver(bool status)
    {
        Instance.isGameOver = status;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    // SerializedClass SaveData
    [Serializable]
    public class SaveData
    {
        public List<string> highScores = new List<string>();
    }

    public void SaveScore()
    {
        var saveFilePath = GetSaveFilePath();

        Debug.Log(_player.Name + " -> " + _player.Score);
        var saveData = new SaveData();
        SetHighScore(_player);

        foreach (var player in _players)
        {
            saveData.highScores.Add(player.SaveToString());
        }

        var json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
    }


    public List<Player> LoadScore()
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
        foreach (var player in saveData.highScores)
        {
            _players.Add(Player.CreateFromJson(player));
        }

        InitializePlayers();

        return _players;
    }

    private void InitializePlayers()
    {
        var nbPlayers = _players.Count;
        for (var i = 0; i < MaxHighScores - nbPlayers; i++)
        {
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

        var comparePlayersScores = Comparer<Player>.Create((a, b) => b.Score - a.Score);

        var index = _players.BinarySearch(player, comparePlayersScores);
        if (index < 0)
        {
            Debug.Log(index + " " + ~index);
            index = ~index;
        }

        _players.Insert(index, player);

        if (_players.Count > MaxHighScores)
        {
            _players.RemoveAt(MaxHighScores);
        }
    }

    private static string GetSaveFilePath()
    {
        return Application.persistentDataPath + "/savefile.json";
    }
}