using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioClip[] runeSelect;

    [SerializeField]AudioSource runeSelectAudioSource;
    [SerializeField] AudioSource runeBoomAudioSource;
    private void Awake()
    {
        instance = this;
    }


    public void PlayRuneSound()
    {
        runeSelectAudioSource.clip = runeSelect[UnityEngine.Random.Range(0, runeSelect.Length - 1)];
        runeSelectAudioSource.Play();
    }
    public void PlayRuneBoomSound()
    {
        runeBoomAudioSource.Play();
    }
}
