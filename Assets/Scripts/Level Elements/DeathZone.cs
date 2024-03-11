using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void Start()
    {
        if (gameObject.name == "FallingDeathZone")
        {
            transform.position = new Vector3(0, -500, 0); //In case there's no gameManager, during testing
            float fdzY = GameObject.Find("GameManager").GetComponent<GameManager>().curLevelInfo.fallingDeathZoneY;
            transform.position = new Vector3(0, fdzY, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
        }
    }
}
