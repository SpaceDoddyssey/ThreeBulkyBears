using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraFollowPoint;
    private float minYVal;
    public CinemachineVirtualCamera cmCam;
    public float shakeStrengthMult, shakeDurationMult, maxShakeDuration;

    void Start()
    {
        cameraFollowPoint = GameObject.Find("CameraFollowPoint").transform;
        mainCamera = GetComponent<Camera>();
        minYVal = GameObject.Find("FallingDeathZoneLoc").transform.position.y + mainCamera.orthographicSize + 1;
        transform.position = GameObject.Find("BearSpawnLoc").transform.position + new Vector3(0, 0, -10);
    }

    void FixedUpdate()
    {
        cmCam.transform.position = new Vector3(cmCam.transform.position.x, cmCam.transform.position.y, -10f);
        if (cameraFollowPoint.transform.position.y < minYVal)
        {
            cameraFollowPoint.position = new Vector3(cameraFollowPoint.position.x, minYVal, cameraFollowPoint.position.z);
        }
    }

    public void Shake(float shakeIntensity, float shakeDuration)
    {
        if (shakeDuration > maxShakeDuration)
        {
            shakeDuration = maxShakeDuration;
        }
        StartCoroutine(_ProcessShake(shakeIntensity * shakeStrengthMult, shakeDuration * shakeDurationMult));
    }

    private IEnumerator _ProcessShake(float shakeIntensity, float shakeDuration)
    {
        float elapsedTime = 0f;
        float startIntensity = shakeIntensity;
        if (startIntensity < 1.5f)
        {
            startIntensity *= 2;
        }
        float endIntensity = 0f;

        while (elapsedTime < shakeDuration)
        {
            float t = elapsedTime / shakeDuration;
            float lerpIntensity = Mathf.Lerp(startIntensity, endIntensity, t);

            Noise(lerpIntensity, lerpIntensity);
            Debug.Log("lerpIntensity: " + lerpIntensity);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Noise(0, 0);
        cmCam.enabled = false;
        cmCam.enabled = true;
        Vector3 bearPos = GameObject.Find("Bear").transform.position;
        cmCam.transform.position = new Vector3(bearPos.x, bearPos.y, bearPos.z - 10);
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }
}