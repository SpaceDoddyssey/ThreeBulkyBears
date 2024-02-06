using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform bear;
    public Vector3 offset;
    public Camera maincamera;

    void Start() 
    {
        maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        bear = GameObject.Find("Bear").transform;
        offset = new Vector3 (0f, 0f, -10f);
    }

    void Update()
    {
        transform.position = new Vector3 (bear.position.x + offset.x, bear.position.y + offset.y, offset.z);
    }

}


