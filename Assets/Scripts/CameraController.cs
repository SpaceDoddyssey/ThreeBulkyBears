using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform bear;
    public Vector3 offset;
    public Camera maincamera;
    public float minYVal;

    void Start()
    {
        maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        bear = GameObject.Find("Bear").transform;
        offset = new Vector3(0f, 0f, -10f);
    }

    void Update()
    {
        transform.position = new Vector3(bear.position.x + offset.x, bear.position.y + offset.y, offset.z);
        if (transform.position.y < minYVal)
        {
            transform.position = new Vector3(transform.position.x, minYVal, transform.position.z);
        }
    }

}


