using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameSettings gameSettings;
    [SerializeField] AudioMixerSnapshot muteSnapshot;
    [SerializeField] AudioMixerSnapshot defaultSnapshot;
    [SerializeField] AudioMixerSnapshot backgroundMuteSnapshot;
    [SerializeField] AudioMixerSnapshot sFXMuteSnapshot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameSettings.MuteAllAudio) // check audio should be mute
            muteSnapshot.TransitionTo(0.1f);
        else
            defaultSnapshot.TransitionTo(0.1f);

    }

    public void ToggleMute()
    {
        if (gameSettings.MuteAllAudio) // unmute if muted
        {
            defaultSnapshot.TransitionTo(0.1f);
            gameSettings.MuteAllAudio = false;
        }
        else // mute if was audible
        {
            muteSnapshot.TransitionTo(0.1f);
            gameSettings.MuteAllAudio = true;
        }
    }
}
