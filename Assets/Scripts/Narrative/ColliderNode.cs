using UnityEngine;
using System.Collections;

namespace DeceptionNarrative
{
    /// <summary>
    /// A node using a collider to trigger NarrativeNode
    /// </summary>

    [RequireComponent(typeof(NarrativeNode), typeof(Collider))]
    public class ColliderNode : MonoBehaviour
    {
        // NarrativeNode triggered by this element
        private NarrativeNode narrativeNode = null;

        void Start()
        {
            narrativeNode = GetComponent<NarrativeNode>();
        }

        private void OnTriggerEnter(Collider collider)
        {
            // If the player collided with the node, attempt to trigger it (the node does all the checks before triggering the narraive event)
            if (collider.tag == Theatre.Tags.TAG_PLAYER)
            {
                narrativeNode.TriggerActivated();
            }

            // If the node has been activated, disable the collider, as it's no longer needed
            if (narrativeNode.Triggered)
            {
                gameObject.collider.enabled = false;
            }
        }
    }
}