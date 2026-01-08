using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptble Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public AudioMixer audioMixer;

    [SerializeField] bool muteAllAudio = false;
    public bool MuteAllAudio
    {
        get { return muteAllAudio; }
        set { muteAllAudio = value; }
    }

    [SerializeField] float muteVolume = -80f;
    public float MuteVolume
    {
        get { return muteVolume; }
        set { muteVolume = value; }
    }
    [SerializeField] float maxVolume = 0f;
    public float MaxVolume
    {
        get { return maxVolume; }
        set { maxVolume = value; }
    }

}
