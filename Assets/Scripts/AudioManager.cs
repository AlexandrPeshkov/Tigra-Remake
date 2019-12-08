using Assets.Scripts.Audio.Subtitles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    public Transform Canvas;

    public List<AudioClip> clipsCorrectly;
    public List<AudioClip> clipsWrong;
    public List<AudioClip> clipsSelect;
    public List<AudioClip> clipsBuildWord;
    public List<AudioClip> clipsFourExcess;
    public List<AudioClip> clipsSunClock;
    public List<AudioClip> clipsAnimals;
    public List<AudioClip> clipsHouses;

    private void Awake()
    {/*
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);*/
        instance = this;
    }

    private void Play(List<AudioClip> clips)
    {
        var clip = clips[Random.Range(0, clips.Count)];
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayCorrectly()
    {

        Play(clipsCorrectly);
        SubtitlesService.Instance.ShowSubtitles("молодец.wav", Canvas, null);
    }

    public void PlayWrong()
    {
        Play(clipsWrong);
        SubtitlesService.Instance.ShowSubtitles("неправильно.wav", Canvas, null);
    }

    public void PlaySelect()
    {
        Play(clipsSelect);
        SubtitlesService.Instance.ShowSubtitles("вступление 1.wav", Canvas, new string[] { "меню фонематика", "игра животные"});
    }

    public void PlayBuildWord()
    {
        Play(clipsBuildWord);
    }

    public void PlayFourExcess()
    {
        Play(clipsFourExcess);
    }

    public void PlaySunClock()
    {
        Play(clipsSunClock);
    }

    public void PlayAnimals()
    {
        Play(clipsAnimals);
    }

    public void PlayHouses()
    {
        Play(clipsHouses);
    }
}
