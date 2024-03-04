using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraFollowPoint;
    private float minYVal;
    public CinemachineVirtualCamera cmCam;
    public float shakeStrengthMult, shakeDurationMult, maxShakeIntesity, maxShakeDuration;
    public float recoveryDuration;

    void Start()
    {
        cameraFollowPoint = GameObject.Find("CameraFollowPoint").transform;
        mainCamera = GetComponent<Camera>();
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

    public void Shake(float shakeIntensity, float shakeDuration)
    {
        Debug.Log("Shake intensity: " + shakeIntensity + " Shake duration: " + shakeDuration);
        if (shakeDuration > maxShakeDuration)
        {
            shakeDuration = maxShakeDuration;
        }
        if (shakeIntensity > maxShakeIntesity)
        {
            shakeIntensity = maxShakeIntesity;
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

        //Reset to resting rotation
        elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.identity;

        while (elapsedTime < recoveryDuration)
        {
            float t = elapsedTime / recoveryDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }
}