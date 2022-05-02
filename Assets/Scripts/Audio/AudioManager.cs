using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    static bool initialized = false;
    static AudioSource audioSource;
    static Dictionary<AudioClipName, AudioClip> audioClips =
        new Dictionary<AudioClipName, AudioClip>();

    bool looping = false;

    /// <summary>
    /// Gets whether or not the audio manager has been initialized
    /// </summary>
    public static bool Initialized
    {
        get { return initialized; }
    }

    /// <summary>
    /// Initializes the audio manager
    /// </summary>
    /// <param name="source">audio source</param>
    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;
        audioClips.Add(AudioClipName.Battle,
            Resources.Load<AudioClip>("Battle"));
        audioClips.Add(AudioClipName.Fireball,
            Resources.Load<AudioClip>("FireBall"));
        audioClips.Add(AudioClipName.Title,
            Resources.Load<AudioClip>("Title"));
        audioClips.Add(AudioClipName.Overworld,
            Resources.Load<AudioClip>("Overworld"));
        audioClips.Add(AudioClipName.Forest,
            Resources.Load<AudioClip>("Forest"));
        audioClips.Add(AudioClipName.Village,
            Resources.Load<AudioClip>("Village"));
        audioClips.Add(AudioClipName.Victory,
            Resources.Load<AudioClip>("Victory"));
        audioClips.Add(AudioClipName.Heal,
            Resources.Load<AudioClip>("Heal"));
    }

    /// <summary>
    /// Plays the audio clip with the given name
    /// </summary>
    /// <param name="name">name of the audio clip to play</param>
    public static void Play(AudioClipName name)
    {
        Debug.Log("Played: " + name);
        audioSource.PlayOneShot(audioClips[name]);
        //audioSource.Play(0);
    }

    public static void ToggleLoop(bool soundLoop)
    {
        audioSource.loop = soundLoop;
    }

    public static void Stop()
    {
        audioSource.Stop();
    }
}
