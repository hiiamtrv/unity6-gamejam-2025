using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource BGM = new AudioSource();
    [SerializeField] private AudioSource UnderWaterSound = new AudioSource();
    [SerializeField] public AudioClip BGMClip;
    [SerializeField] public AudioClip UnderWaterClip;
    [SerializeField] private float BGMvolume = 0.4f;
    [SerializeField] private float UWSVolume = 0.3f;
    [SerializeField] private bool isPlaying;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        BGM.clip = BGMClip;
        BGM.volume = BGMvolume;
        UnderWaterSound.clip = UnderWaterClip;
        UnderWaterSound.volume = UWSVolume;
        BGM.loop = true;
        UnderWaterSound.loop = true;
        BGM.Play();
        UnderWaterSound.Play();
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying == true)
        {
            PauseBGM();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPlaying == false)
        {
            PlayBGM();
        }
    }
    public void PauseBGM()
    {
        BGM.Pause();
        UnderWaterSound.Pause();
        isPlaying=false;
    }
    public void PlayBGM()
    {
        BGM.UnPause();
        UnderWaterSound.UnPause();
        isPlaying = true;
    }
}
