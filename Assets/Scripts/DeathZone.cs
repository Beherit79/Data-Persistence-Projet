using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeathZone : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        GameManager.Instance.SaveScore();
        GameManager.SetGameOver(true);
    }
}
