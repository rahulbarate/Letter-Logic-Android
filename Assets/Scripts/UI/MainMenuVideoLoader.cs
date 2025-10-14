using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class MainMenuVideoLoader : MonoBehaviour
{
    public List<VideoPlayer> videoPlayers;
    public Texture2D loadingTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (VideoPlayer videoPlayer in videoPlayers)
        {
            RawImage rawImage = videoPlayer.transform.parent.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = loadingTexture;
            }
            videoPlayer.prepareCompleted += OnPrepareCompleted;
            videoPlayer.Prepare();
        }
    }

    void OnPrepareCompleted(VideoPlayer source)
    {
        source.prepareCompleted -= OnPrepareCompleted;
        RawImage rawImage = source.transform.parent.GetComponent<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = source.targetTexture;
        }
        source.Play();
    }
}
