using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NarrativeNode : MonoBehaviour
{
    [SerializeField]
    private SubtitledAudio[] audioPlaylist; // All subtitled audio files to play in order
    [SerializeField]
    private NarrativeNode[] prerequisites; // All other nodes that need to be triggered before this one becomes active

    // Was this node previosuly triggered
    public bool Triggered { get; private set; }
    // Called when this node triggers
    public UnityAction OnNodeTriggered { get; set; }

    // Has this node finished playing all the audio
    public bool FinishedPlaying { get; private set; }
    // Called when this node finished playing all audio
    public UnityAction OnNodeFinishedPlaying { get; set; }

    public void TriggerNode()
    {
        // If the node wasn't previously triggered, play the audio
        if (!Triggered)
        {
            Triggered = true;
            StartCoroutine(Play());
        }
    }

    private IEnumerator Play()
    {
        foreach(SubtitledAudio audio in audioPlaylist)
        {

            yield return new WaitForSeconds(1f);
        }
    }
}
