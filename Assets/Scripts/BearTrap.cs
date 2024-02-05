using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: change to closed sprite if triggered
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
        }
    }
}
