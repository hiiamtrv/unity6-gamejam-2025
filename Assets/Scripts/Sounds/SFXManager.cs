using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; } 
    public AudioSource Player;
    public AudioClip[] SFX;
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
    private void LoadCLipIntoPlayer(int Index)
    {
        Player.clip = SFX[Index];
    }
    public void PlayPopSound()
    {
        Player.PlayOneShot(SFX[0], 1f);
    }
    public void PlayDisappointedSound()
    {
        int rng = Random.Range(1, 7);
        Player.PlayOneShot(SFX[rng], 1f);
    }
    public void PlayHappySound()
    {
        int rng = Random.Range(7, 11);
        Player.PlayOneShot(SFX[rng], 1f);
    }
}
