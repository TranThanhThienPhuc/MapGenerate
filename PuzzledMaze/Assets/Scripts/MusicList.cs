using UnityEngine;

public class MusicList : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] songs;
    public int index;

    private void Start()
    {
        audioSource.clip = songs[index];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextMusic();
        }
    }
    void PlayNextMusic()
    {
        if (index == 3) index = 0;
        else index++;
        audioSource.clip = songs[index];
        audioSource.Play();

    }
}
