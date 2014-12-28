using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

public class NarrativeNode : MonoBehaviour
{
    // All subtitled audio files to play in order
    [SerializeField]
    private SubtitledAudio[] audioPlaylist = null;
    // All other nodes that need to be triggered before this one becomes active
    [SerializeField]
    private NarrativeNode[] prerequisites = null;

    // Was this node previosuly triggered
    public bool Triggered { get; private set; }
    // Called when this node triggers
    public System.Action OnNodeTriggered { get; set; }

    // Has this node finished playing all the audio
    public bool FinishedPlaying { get; private set; }
    // Called when this node finished playing all audio
    public System.Action OnNodeFinishedPlaying { get; set; }

    private static NarrativeNodePlayQueue playQueue = null;

    // AudioSource component that will play the audio clips
    private static AudioSource audioSource = null;

    private void Awake()
    {
        // If the queue has not been initialised yet, do it
        if (playQueue == null)
        {
            playQueue = new NarrativeNodePlayQueue();
        }

        // Create a game object to play will play the audio
        if (audioSource == null)
        {
            GameObject go = new GameObject("NarrativeAudio", typeof(AudioSource));
            audioSource = go.GetComponent<AudioSource>();
        }
    }

    // Let the node know that the assigned trigger has been activated
    public void TriggerActivated()
    {
        // If the node wasn't previously triggered and all prerequisites have triggered, push the contents onto the queue to be played
        if (!Triggered && prerequisites.All(p => p.Triggered))
        {
            // Send out the trigger event
            Triggered = true;
            if (OnNodeTriggered != null)
            {
                OnNodeTriggered();
            }

            playQueue.PushToPlayQueue(this);
        }
    }

    // Play the contents of the node
    private IEnumerator Play()
    {
        GameController gameController = GameController.Controller;

        foreach (SubtitledAudio audio in audioPlaylist)
        {
            // Give control to the SubtitledAudio instance to play itself out, push the subtitles to the GameController to propagate
            yield return StartCoroutine(audio.Play(audioSource, gameController.SubtitlesToDisplay));
            // Send out an empty string at the end, so the subtitles don't linger
            gameController.SubtitlesToDisplay(string.Empty);
        }

        // Send out the completion event.
        FinishedPlaying = true;
        if (OnNodeFinishedPlaying != null)
        {
            OnNodeFinishedPlaying();
        }
    }

    #region EditorGizmos
    private void OnDrawGizmos()
    {
        // Draw lines to all parents with marking showing the direction of the relationship
        foreach (NarrativeNode node in prerequisites)
        {
            if (node != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, node.transform.position);

                Vector3 toChild = transform.position - node.transform.position;
                Vector3 childIconPoint = transform.position - toChild * 0.3f;
                Vector3 parentIconPoint = node.transform.position + toChild * 0.3f;

                Gizmos.DrawIcon(childIconPoint, "C.png", false);
                Gizmos.DrawIcon(parentIconPoint, "P.png", false);
            }
        }
    }
    #endregion

    // Queue managing playing of the node contents
    private class NarrativeNodePlayQueue
    {
        // Queue of triggered nodes waiting to be played
        private Queue<NarrativeNode> nodesToPlay = null;
        public bool IsQueuePlaying { get; private set; }

        public NarrativeNodePlayQueue()
        {
            nodesToPlay = new Queue<NarrativeNode>();
        }

        // Add the node to the queue to be played when previous nodes are done
        public void PushToPlayQueue(NarrativeNode node)
        {
            nodesToPlay.Enqueue(node);
            // If the queue is not already running, start it now
            if (!IsQueuePlaying)
            {
                node.StartCoroutine(PlayQueue());
                IsQueuePlaying = true;
            }
        }

        // Play all the nodes currently in the queue
        private IEnumerator PlayQueue()
        {
            while (nodesToPlay.Count > 0)
            {
                NarrativeNode node = nodesToPlay.Dequeue();
                yield return node.StartCoroutine(node.Play());
            }

            IsQueuePlaying = false;

        }
    }
}
