using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//Code sourced from Max O'Didily on Youtube at https://www.youtube.com/watch?v=9UE3hLSHMTE
public class SplashVidPlayer : MonoBehaviour
{
    [SerializeField] private string videoFileName;

    // Start is called before the first frame update
    void Start()
    {
        PlayVideo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}
