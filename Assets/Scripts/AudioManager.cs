using UnityEngine.Audio;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start() {
        AudioManager.Play("MainMenuSong");
    }

    public static Sound FindSound(string name) {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning($"Sound: {name} not found!");
        }
        return s;
    }

    public static void StartPlaying(string name) {
        Sound s = FindSound(name);
        if(s == null) {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
        if (!s.source.isPlaying) s.source.Play();
    }

    public static void StopPlaying(string name) {
        Sound s = FindSound(name);
        if (s == null) {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
        s.source.Pause();
    }

    public static void Play(string name) {
        Sound s = FindSound(name);
        if(s == null) {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
        s.source.Play();
    }
}
