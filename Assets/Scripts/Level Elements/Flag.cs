using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    void Start()
    {
        GameObject flagLoc = GameObject.Find("FlagLoc");
        flagLoc.GetComponent<SpriteRenderer>().enabled = false;
        transform.position = flagLoc.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().Victory();
        }
    }
}
