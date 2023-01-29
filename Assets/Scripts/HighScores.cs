using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HighScores : MonoBehaviour
{
    private List<Player> _highScores;

    public TextMeshProUGUI playerXTextPrefab;

    public TextMeshProUGUI playerXHighScoreTextPrefab;

    // initialize the high scores from persistent data
    private void Start()
    {
        // initialize example _highScores
        _highScores = GameManager.Instance.LoadScore() ?? new List<Player>();
        Debug.Log(_highScores);
        DisplayHighScores();
    }

    private void DisplayHighScores()
    {
        // sort high scores by descending score
        _highScores = _highScores.OrderByDescending(player => player.Score).ToList();

        var playerTextYOffset = playerXTextPrefab.transform.position.y;
        var highScoreTextYOffset = playerXHighScoreTextPrefab.transform.position.y;
        foreach (var player in _highScores)
        {
            // Instantiate the high score text objects
            var playerText = Instantiate(playerXTextPrefab, transform);
            var highScoreText = Instantiate(playerXHighScoreTextPrefab, transform);

            // set the high score text position
            var playerTextPos = playerText.transform.position;
            playerTextPos = new Vector3(playerTextPos.x, playerTextYOffset, playerTextPos.z);
            playerText.transform.position = playerTextPos;

            var highScoreTextPost = highScoreText.transform.position;
            highScoreTextPost = new Vector3(highScoreTextPost.x, highScoreTextYOffset, highScoreTextPost.z);
            highScoreText.transform.position = highScoreTextPost;

            // set the high score text
            playerText.text = player.Name;
            highScoreText.text = player.Score.ToString();

            // move the high score text position down
            playerTextYOffset -= 50f;
            highScoreTextYOffset -= 50f;
        }
    }
}