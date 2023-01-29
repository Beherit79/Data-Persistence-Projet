using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        var player = GameManager.Instance.GetCurrentPlayer();
        GameManager.Instance.SaveScore(player);
        GameManager.SetGameOver(true);
    }
}