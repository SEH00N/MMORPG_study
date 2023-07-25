using System.Security.Claims;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // MP3 -> AudioSource
    // 음악 -> AudioClip
    // 관객(귀) -> AudioListener

    private AudioSource[] audioSources = new AudioSource[(int)DEFINE.Sound.Length];
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("Sound");
        if(root == null)
        {
            root = new GameObject("Sound");
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(DEFINE.Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject(soundNames[i]);
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.SetParent(root.transform);
            }

            audioSources[(int)DEFINE.Sound.BGM].loop = true;
        }
    }

    public void Clear()
    {
        foreach(AudioSource source in audioSources)
        {
            source.clip = null;
            source.Stop();
        }

        audioClips.Clear();
    }

    public void Play(string path, DEFINE.Sound type = DEFINE.Sound.Effect, float pitch = 1f)
    {
        AudioClip clip = GetOrAddAudioClip(path, type);
        Play(clip, type, pitch);
    }

    public void Play(AudioClip clip, DEFINE.Sound type = DEFINE.Sound.Effect, float pitch = 1f)
    {
        if(clip == false)
            return;

        if(type == DEFINE.Sound.BGM)
        {
            AudioSource source = audioSources[(int)DEFINE.Sound.BGM];
            if (source.isPlaying)
                source.Stop();

            source.pitch = pitch;
            source.clip = clip;
            source.Play();
        }
        else
        {
            AudioSource source = audioSources[(int)DEFINE.Sound.Effect];
            source.pitch = pitch;
            source.PlayOneShot(clip);
        }
    }

    private AudioClip GetOrAddAudioClip(string path, DEFINE.Sound type)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip clip = null;

        if(type == DEFINE.Sound.BGM)
            clip = Managers.Resource.Load<AudioClip>(path);
        else
        {
            if (audioClips.TryGetValue(path, out clip) == false)
            {
                clip = Managers.Resource.Load<AudioClip>(path);
                audioClips.Add(path, clip);
            }
        }

        if (clip == null)
            Debug.Log($"Audio Clip is Missing! {path}");

        return clip;
    }
}
