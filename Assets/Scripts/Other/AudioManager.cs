using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameSettings gameSettings;
    [SerializeField] AudioMixerSnapshot muteSnapshot;
    [SerializeField] AudioMixerSnapshot defaultSnapshot;
    [SerializeField] AudioMixerSnapshot backgroundMuteSnapshot;
    [SerializeField] AudioMixerSnapshot sFXMuteSnapshot;
    [SerializeField] AudioMixerSnapshot gameWonSnapshot;
    [SerializeField] AudioMixerSnapshot gameOverSnapshot;

    [SerializeField] AudioSource backgroundMusicAudioSource;
    [SerializeField] AudioSource gameWonAudioSource;
    [SerializeField] AudioSource fireworkAudioSource;
    [SerializeField] AudioSource gameOverAudioSource;
    [SerializeField] AudioSource powerUpAudioSource;
    [SerializeField] AudioSource milestoneAudioSource;
    [SerializeField] AudioSource purchaseAudioSource;

    [SerializeField] AudioClip[] mainMenuBackgroundAudioClips;
    [SerializeField] AudioClip[] playgroundBackgroundAudioClips;

    AudioMixerSnapshot[] allSnapshots;
    float[] resetWeights;
    public static AudioManager instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);


            AudioSettings.GetConfiguration(); // forces listener creation
            audioMixer.SetFloat("MasterVolume", 0f); // forces runtime binding
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allSnapshots = new AudioMixerSnapshot[]
        {
            defaultSnapshot,
            muteSnapshot,
            backgroundMuteSnapshot,
            sFXMuteSnapshot,
            gameWonSnapshot,
            gameOverSnapshot
        };
        resetWeights = new float[allSnapshots.Length];
        SceneManager.activeSceneChanged += OnSceneChanged;
        SetRandomBackgroundMusic();
        MuteCheck();


    }

    void OnSceneChanged(Scene curr, Scene next)
    {
        SetRandomBackgroundMusic();
        MuteCheck();
    }

    void SetRandomBackgroundMusic()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            backgroundMusicAudioSource.resource = mainMenuBackgroundAudioClips[UnityEngine.Random.Range(0, mainMenuBackgroundAudioClips.Length)];
            backgroundMusicAudioSource.Play();
        }
        else
        {
            backgroundMusicAudioSource.resource = playgroundBackgroundAudioClips[UnityEngine.Random.Range(0, playgroundBackgroundAudioClips.Length)];
            backgroundMusicAudioSource.Play();
        }
    }

    void MuteCheck()
    {
        if (gameSettings.MuteAllAudio) // check audio should be mute
            GoToSnapshot(muteSnapshot, 0f);
        else
            GoToSnapshot(defaultSnapshot, 0f);
    }

    public void ToggleMute()
    {
        if (gameSettings.MuteAllAudio) // unmute if muted
        {
            GoToSnapshot(defaultSnapshot);
            gameSettings.MuteAllAudio = false;
        }
        else // mute if was audible
        {
            GoToSnapshot(muteSnapshot);
            gameSettings.MuteAllAudio = true;
        }
    }

    public void PlayGameWonSFX()
    {
        if (!gameSettings.MuteAllAudio)
        {
            GoToSnapshot(gameWonSnapshot);
            gameWonAudioSource.Play();
            fireworkAudioSource.Play();
        }
    }
    public void PlayGameOverSFX()
    {
        if (!gameSettings.MuteAllAudio)
        {
            GoToSnapshot(gameOverSnapshot, 0f);
            gameOverAudioSource.Play();
        }
        // CustomLogger.Log("PlayGameOverSFX");
    }
    public void PlayPowerUpSFX()
    {
        if (!gameSettings.MuteAllAudio)
            powerUpAudioSource.Play();
    }
    public void PlayPurchaseSFX()
    {
        if (!gameSettings.MuteAllAudio)
            purchaseAudioSource.Play();
    }
    public void PlayMilestoneSFX()
    {
        if (!gameSettings.MuteAllAudio)
            milestoneAudioSource.Play();
    }
    public void TransitionToDefault()
    {
        GoToSnapshot(defaultSnapshot);
        MuteCheck();
        // if (!gameSettings.MuteAllAudio)
        //     backgroundMusicAudioSource.Play();
    }

    void GoToSnapshot(AudioMixerSnapshot target, float duration = 0.1f)
    {
        for (int i = 0; i < allSnapshots.Length; i++)
            resetWeights[i] = (allSnapshots[i] == target) ? 1f : 0f;
        audioMixer.TransitionToSnapshots(allSnapshots, resetWeights, duration);
    }
}
