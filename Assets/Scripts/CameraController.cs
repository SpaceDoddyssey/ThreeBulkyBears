using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform bear;
    private Vector3 offset;
    private Camera mainCamera;
    private float minYVal;
    private LevelManager levelManager;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        bear = GameObject.Find("Bear").transform;
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        offset = new Vector3(0f, 0f, -10f);
        minYVal = GameObject.Find("FallingDeathZoneLoc").transform.position.y + mainCamera.orthographicSize + 1;
    }

    void Update()
    {
        if (levelManager.lost) { return; }
        transform.position = new Vector3(bear.position.x + offset.x, bear.position.y + offset.y, offset.z);
        if (transform.position.y < minYVal)
        {
            transform.position = new Vector3(transform.position.x, minYVal, transform.position.z);
        }
    }

}


