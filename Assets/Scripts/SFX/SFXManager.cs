using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; } 
    public AudioSource[] RandomAudio;
    public AudioSource[] HappyClips;
    public AudioSource[] DisappointedClips;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Another instance of SingletonClass exists! Destroying this one.");
            Destroy(gameObject);
        }
    }
    public void PlayPopSound()
    {
        RandomAudio[0].Play();
    }
    public void PlayDisappointedSound()
    {
        DisappointedClips[0].Play();
    }
    public void PlayHappySound()
    {
        HappyClips[0].Play();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            PlayPopSound();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            PlayHappySound();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            PlayDisappointedSound();
        }

    }
}
