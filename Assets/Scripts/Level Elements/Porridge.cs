using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porridge : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnLoc = GameObject.Find("PorridgeSpawnLoc");
        spawnLoc.GetComponent<SpriteRenderer>().enabled = false;

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.curLevelInfo.hasPorridge == false
            || PlayerPrefs.GetInt(gameManager.curLevelInfo.levelName + "PorridgeCollected") == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        if (spawnLoc != null)
        {
            transform.position = spawnLoc.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Just Right!");
            PlayerPrefs.SetInt(gameManager.curLevelInfo.levelName + "PorridgeCollected", 1);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<ParticleSystem>().Play();
        }
    }
}