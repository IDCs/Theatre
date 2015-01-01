using UnityEngine;
using System.Collections;

namespace DeceptionNarrative
{
    [RequireComponent(typeof(NarrativeNode), typeof(CObject))]
    public class ObjectNode : MonoBehaviour
    {
        // NarrativeNode triggered by this element
        private NarrativeNode narrativeNode = null;

        // CObject this node is observing
        private CObject interactableObject = null;

        private void Start()
        {
            narrativeNode = GetComponent<NarrativeNode>();
            interactableObject = GetComponent<CObject>();

            // subscribe to pick up and drag events of the object
            interactableObject.OnObjectCollected += (item) => ObjectInteractionStarted();
            interactableObject.OnObjectDragged += (go) => ObjectInteractionStarted();
        }

        private void ObjectInteractionStarted()
        {
            // Attempt to trigger the narrative node
            narrativeNode.TriggerActivated();
        }
    }
}
