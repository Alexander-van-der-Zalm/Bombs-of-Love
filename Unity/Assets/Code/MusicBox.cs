using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicBox : MonoBehaviour
{
    public List<AudioClip> clips;
    private AudioSource source;
    private int currentIndex = -1;

    public void Awake()
    {
        source = GetComponent<AudioSource>();

        PlayNewRandom();
    }

    public void Play(int index = 0)
    {
        source.clip = clips[index];
        currentIndex = index;
        source.Play();
        Debug.Log("Playing: " + clips[index].name);
        source.loop = true;
    }

    public void PlayNewRandom()
    {
        int newRandom = (int)Random.Range(0, clips.Count);
        if(newRandom == currentIndex && clips.Count > 1)
        {
            PlayNewRandom();
            return;
        }
        Play(newRandom);
        // Use clip length coroutine to max the loops
    }

}
