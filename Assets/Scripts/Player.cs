using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Player
{
    [SerializeField] private int score;
    [SerializeField] private string name;

    public Player(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    // public int GetScore()
    // {
    //     return _score;
    // }
    //
    // public void SetScore(int score)
    // {
    //     _score = score;
    // }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public int Score
    {
        get => score;
        set => score = value;
    }

    public static Player CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<Player>(jsonString);
    }

    public string SaveToString()
    {
        Debug.Log(this.Name + " " + JsonUtility.ToJson(this));
        return JsonUtility.ToJson(this);
    }
}