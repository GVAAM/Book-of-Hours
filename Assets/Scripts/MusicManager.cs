using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public List<AudioClip> MusicList = new List<AudioClip>();
    [SerializeField] public List<string> MusicNameList = new List<string>();

    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //update music in control panel
    public void UpdateTrack(int i)
    {
        AudioClip clip = MusicList[i];
        audioSource.clip = clip;
        audioSource.Play();
    }
}
