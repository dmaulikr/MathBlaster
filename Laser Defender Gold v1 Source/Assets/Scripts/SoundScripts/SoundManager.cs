using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip playerFire1;
    public AudioClip enemyFire1;
    public AudioClip explosion1;
    public AudioClip playerExplosion;
    public AudioClip powerUpRepair;
    public AudioClip powerUpAnswer;
	public static SoundManager instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a Sound Player
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
