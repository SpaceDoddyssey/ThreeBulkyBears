using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porridge : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Porridge Start");
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.curLevelInfo.hasPorridge == false
            || PlayerPrefs.GetInt(gameManager.curLevelInfo.levelName + "PorridgeCollected") == 1)
        {
            Debug.Log("Porridge not in level or already collected");
            gameObject.SetActive(false);
            return;
        }

        GameObject spawnLoc = GameObject.Find("PorridgeSpawnLoc");
        if (spawnLoc != null)
        {
            Debug.Log("Porridge SpawnLoc found");
            transform.position = spawnLoc.transform.position;
            spawnLoc.GetComponent<SpriteRenderer>().enabled = false;
        }

        Debug.Log("Porridge End");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Just Right!");
            PlayerPrefs.SetInt(gameManager.curLevelInfo.levelName + "PorridgeCollected", 1);
            gameObject.SetActive(false);
        }
    }
}