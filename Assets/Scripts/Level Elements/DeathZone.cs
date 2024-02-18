using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void Start()
    {
        if (gameObject.name == "FallingDeathZone")
        {
            GameObject fdzLoc = GameObject.Find("FallingDeathZoneLoc");
            if (fdzLoc != null)
            {
                transform.position = fdzLoc.transform.position;
            }
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
