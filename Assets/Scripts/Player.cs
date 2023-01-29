using System;
using UnityEngine;

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

    public Player GetPlayer()
    {
        return this;
    }

    public static Player CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<Player>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void AddPoints(int points)
    {
        var newScore = Score + points;
        Score = newScore;
    }

    public void ResetPoint()
    {
        Score = 0;
    }
}