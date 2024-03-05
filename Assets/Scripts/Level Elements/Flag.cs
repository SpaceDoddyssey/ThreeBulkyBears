using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField]
    private Sprite poofSmoke;

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
            StartCoroutine(Poof());
        }
    }

    public IEnumerator Poof()
    {
        GetComponent<AudioSource>().Play();

        GetComponent<SpriteRenderer>().sprite = poofSmoke;

        yield return new WaitForSeconds(0.4f);

        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        // Ensure the smoke is completely invisible
        transform.localScale = targetScale;
    }
}
