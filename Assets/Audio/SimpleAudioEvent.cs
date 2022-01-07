using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;
    [MinMaxRange(0, 2)]
    public RangedFloat pitch;

    //[MinMaxRange(0, 2)]
    public RangedFloat volume;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) return;
        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.Play();
    }

    public override void PlayWithParameters(AudioSource source, float volume, float pitch)
    {
        if (clips.Length == 0) return;
        //int timer = (int)Mathf.Round((Time.realtimeSinceStartup / 15) % 6);  //Count from 0 to 6? changing every 15 seconds
        //source.clip = clips[Random.Range(0, clips.Length)];
        //source.clip = clips[timer];
        source.clip = clips[0];
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }
}
