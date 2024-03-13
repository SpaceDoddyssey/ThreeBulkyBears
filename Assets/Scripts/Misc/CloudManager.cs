using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    private List<GameObject> cloudObjs;
    public int gridWidth, gridHeight;
    public float spacing, noiseMult, variance;
    public Vector2 offset;
    public float cloudSpeed;
    private float cloudBoundLeft, cloudBoundRight;

    // Start is called before the first frame update
    void Start()
    {
        cloudBoundLeft = GameObject.Find("GameManager").GetComponent<GameManager>().curLevelInfo.cloudBoundLeft;
        cloudBoundRight = GameObject.Find("GameManager").GetComponent<GameManager>().curLevelInfo.cloudBoundRight;
        spacing = (cloudBoundRight - cloudBoundLeft) / gridWidth;
        cloudObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cloud"));
        DistributeClouds();
    }

    void DistributeClouds()
    {
        for (int i = 0; i < cloudObjs.Count; i++)
        {
            int x = i / gridHeight;
            int y = i % gridHeight;
            GameObject cloud = cloudObjs[i];
            Vector2 position = new Vector2((x + 1) * spacing, y * spacing) + offset;

            cloud.transform.localPosition = position;
        }

        foreach (var cloud in cloudObjs)
        {
            Vector3 tf = cloud.transform.localPosition;

            float dx = Random.Range(-variance, variance);
            float dy = Random.Range(-variance, variance);

            tf.x += dx * noiseMult;
            tf.y += dy * noiseMult;
            tf.z = transform.position.z;

            cloud.transform.localPosition = tf;
        }
    }

    void MoveClouds()
    {
        foreach (var cloud in cloudObjs)
        {
            Vector3 tf = cloud.transform.localPosition;
            tf.x -= cloudSpeed * Time.deltaTime;

            if (tf.x < cloudBoundLeft)
            {
                tf.x = cloudBoundRight;
            }

            cloud.transform.localPosition = tf;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DistributeClouds();
        }

        MoveClouds();
    }
}
