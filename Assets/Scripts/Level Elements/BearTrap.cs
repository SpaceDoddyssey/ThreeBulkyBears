using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public Sprite closedSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = closedSprite;
            GetComponent<AudioSource>().Play();
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
        }
    }
}
