using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubtitledAudio : ScriptableObject
{
    [SerializeField]
    private AudioClip audioClip = null;
    public AudioClip AudioClip { get { return audioClip; } }

    [SerializeField]
    private SubtitlesLine[] subtitles = null;
    public IEnumerable<SubtitlesLine> Subtitles { get { return subtitles; } }


}
