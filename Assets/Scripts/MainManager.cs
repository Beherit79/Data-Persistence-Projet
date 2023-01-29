using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public int lineCount = 6;
    public Rigidbody ball;

    public Text scoreText;
    public Text bestScoreText;
    public GameObject gameOver;

    private int _gamePoints;

    private bool _isGameStarted;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.GetCurrentPlayer().ResetPoint();
        var bestPlayer = GameManager.Instance.GetBestPlayer();
        bestScoreText.text = "Best Score: " + bestPlayer.Name + " - " + bestPlayer.Score; 
        const float step = 0.6f;
        var perLine = Mathf.FloorToInt(4.0f / step);

        var pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (var i = 0; i < lineCount; ++i)
        for (var x = 0; x < perLine; ++x)
        {
            var position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
            var brick = Instantiate(brickPrefab, position, Quaternion.identity);
            brick.pointValue = pointCountArray[i];
            brick.onDestroyed.AddListener(AddPoint);
        }
    }


    private void Update()
    {
        if (!_isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.SetGameOver(false);
                _isGameStarted = true;
                var randomDirection = Random.Range(-1.0f, 1.0f);
                var forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (GameManager.Instance.isGameOver)
        {
            gameOver.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void AddPoint(int point)
    {
        _gamePoints += point;
        scoreText.text = $"Score : {_gamePoints}";
        GameManager.Instance.GetCurrentPlayer().AddPoints(point);
    }
}