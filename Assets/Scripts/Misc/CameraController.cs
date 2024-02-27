using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraFollowPoint;
    private float minYVal;
    private LevelManager levelManager;

    void Start()
    {
        cameraFollowPoint = GameObject.Find("CameraFollowPoint").transform;
        mainCamera = GetComponent<Camera>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        minYVal = GameObject.Find("FallingDeathZoneLoc").transform.position.y + mainCamera.orthographicSize + 1;
        transform.position = GameObject.Find("BearSpawnLoc").transform.position + new Vector3(0, 0, -10);
    }

    void FixedUpdate()
    {
        if (cameraFollowPoint.transform.position.y < minYVal)
        {
            cameraFollowPoint.position = new Vector3(cameraFollowPoint.position.x, minYVal, cameraFollowPoint.position.z);
        }
    }
}