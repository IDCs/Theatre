using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubtitledAudio : ScriptableObject
{
    // Audio clip to be played
    [SerializeField]
    private AudioClip audioClip = null;    
    public AudioClip AudioClip { get { return audioClip; } }

    // Timed subtitles for the audio
    [SerializeField]
    private SubtitlesLine[] subtitles = null;
    public IEnumerable<SubtitlesLine> Subtitles { get { return subtitles; } }

    // Time when last subtitles line started
    private float lastLineTime;
    private float endPadding = 3f; // Time hold the last line for

    private void Start()
    {
        lastLineTime = 0f;
    }

    // Play the clip with subtitles. Supply SudioSource to play the sounds and method to call with content of current subtitles line
    public IEnumerator Play(AudioSource audioSource, System.Action<string> subtitlesCallback)
    {
        audioSource.PlayOneShot(audioClip);

        // Display the subtitles at set times
        foreach(SubtitlesLine line in subtitles)
        {
            yield return new WaitForSeconds(line.Time - lastLineTime);
            subtitlesCallback(line.Text);
            lastLineTime = line.Time;
        }

        // If the clip is still playing, whait for it to finish
        if(audioSource.isPlaying)
        {
            yield return new WaitForSeconds(audioSource.clip.length - audioSource.time);
        }
        else
        {
            yield return new WaitForSeconds(endPadding);
        }

        audioSource.clip = null;
    }
}
