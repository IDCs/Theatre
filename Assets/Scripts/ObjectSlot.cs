using UnityEngine;
using System.Collections;
using System.Linq;
using System;

namespace DeceptionPuzzle
{
    /// <summary>
    /// A slot where a CObject can be placed
    /// It handles detecting potential objects, sending out prompts and animating the object being placed
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ObjectSlot : MonoBehaviour
    {
        private bool CanReceveObjects;

        // Area that will intercept objects
        private Collider triggerArea = null;
        // Position and rotation an object should take when intercepted
        private GameObject restingPoint = null;
        // Time it takes the object to move from drop point to resting point
        private float transitionTime = 1f;

        // Id of the correct object for this slot
        [SerializeField]
        private int correctObjectID = Theatre.Constants.DEFAULT_INVALID_OBJECT_ID;
        // Ids of other objects that can be inserted in this slot
        [SerializeField]
        private int[] otherAcceptedObjectIDs = null;
        // Text to display when player is within the interception area
        [SerializeField]
        private string interactionPrompt = "";

        // Object currently in the slot
        private CObject attachedObject = null;

        // Events
        public Action<bool> OnReceivedObject { get; set; } // Called when an object is put in the slot, passes whether it is the correct object for the slot
        public Action OnObjectRemoved { get; set; } // Called when an object is removed from the slot

        // Store delegates that will be send to the objects, so they can be removed
        private Action<InventoryItemInfo> attachDelegate;
        private Action<InventoryItemInfo> removedDelegate;
        private Action<GameObject> draggedOffDelegate;

        private void Start()
        {
            if (correctObjectID == Theatre.Constants.DEFAULT_INVALID_OBJECT_ID)
            {
                Debug.LogError(string.Format("Object slot - {0} - doesn't have a correct object id assigned", name));
            }

            triggerArea = GetComponent<Collider>();

            restingPoint = transform.GetChild(0).gameObject;
        }

        public void Activate()
        {
            CanReceveObjects = true;
        }

        public void Deactivate()
        {
            CanReceveObjects = false;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (CanReceveObjects)
            {
                CObject cObject = collider.GetComponent<CObject>();
                if (IsObjectValidForSlot(cObject))
                {
                    StartWatchingObject(cObject);
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (CanReceveObjects)
            {
                CObject cObject = collider.GetComponent<CObject>();
                if (IsObjectValidForSlot(cObject))
                {
                    // Stop listening for the object being attached
                    StopWatchingObject(cObject);
                }
            }
        }

        // Returns true if there is an attached object and the id matches the required id, flase otherwise
        public bool HasCorrectObject()
        {
            return attachedObject != null && attachedObject.ObjectId == correctObjectID;
        }

        private void StartWatchingObject(CObject cObject)
        {
            // Wait for the object to be dropped
            attachDelegate = (info) => AttachObject(cObject);
            cObject.OnObjectDropped += attachDelegate;
            // Display prompt
            SendPromptText(interactionPrompt);
        }

        private void StopWatchingObject(CObject cObject)
        {
            // Unregister from drop event
            cObject.OnObjectDropped -= attachDelegate;
            attachDelegate = null;
            // Remove prompt
            SendPromptText(String.Empty);
        }

        // Send propmt text to be displayed on the UI
        private void SendPromptText(string prompt)
        {
            GameController gameController = GameController.Controller;
            gameController.InteractionPromptToDisplay(prompt);
        }

        // Check whether the colliding object could be attached to this slot
        private bool IsObjectValidForSlot(CObject cObject)
        {
            return (cObject != null && (cObject.ObjectId == correctObjectID || otherAcceptedObjectIDs.Contains(cObject.ObjectId)));
        }

        // Attach the passed object to this slot
        private void AttachObject(CObject cObject)
        {
            attachedObject = cObject;

            // Turn off object gravity, so it rests on the assigned point
            // FIXME: this should be probabylt controlle object side, so it can reactivate those when needed
            Rigidbody rigidbody = attachedObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            // Stop listening for the object being attached
            StopWatchingObject(cObject);
            // Register to check if the object is removed later on
            MonitorDetach();

            StartCoroutine(AlignObjectWithRestingPoint());

            //Emit event
            if (OnReceivedObject != null)
            {
                OnReceivedObject(HasCorrectObject());
            }
        }

        // Animate the object moving into the slot
        private IEnumerator AlignObjectWithRestingPoint()
        {
            Vector3 fromPos = attachedObject.transform.position;
            Quaternion fromRot = attachedObject.transform.rotation;
            Vector3 translation = restingPoint.transform.position - fromPos;
            float distance = translation.magnitude;
            float speed = distance / transitionTime;

            float t = 0;

            while (t < 1)
            {
                attachedObject.transform.position = Vector3.Lerp(fromPos, restingPoint.transform.position, t);
                attachedObject.transform.rotation = Quaternion.Slerp(fromRot, restingPoint.transform.rotation, t);
                t += speed * Time.deltaTime;
                yield return null;
            }
        }

        // Register to receive events about the object in this slot being taken away
        private void MonitorDetach()
        {
            if (attachedObject != null)
            {
                // Register for object pickup events
                removedDelegate += (info) => DetachObject();
                draggedOffDelegate += (go) => DetachObject();

                attachedObject.OnObjectCollected += removedDelegate;
                attachedObject.OnObjectDragged += draggedOffDelegate;
            }
        }

        // Remove the object from this slot
        private void DetachObject()
        {
            // Set back rigidbody settings
            Rigidbody rigidbody = attachedObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;

            // unregister delegates
            attachedObject.OnObjectCollected -= removedDelegate;
            attachedObject.OnObjectDragged -= draggedOffDelegate;

            attachedObject = null;

            // Emit event
            if (OnObjectRemoved != null)
            {
                OnObjectRemoved();
            }
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            triggerArea = GetComponent<Collider>();
            Gizmos.color = Color.yellow;

            BoxCollider box = triggerArea as BoxCollider;
            if (box != null)
            {
                Gizmos.DrawWireCube(transform.position + box.center, box.size);
            }

            SphereCollider sphere = triggerArea as SphereCollider;
            if (sphere != null)
            {
                Gizmos.DrawWireSphere(transform.position + sphere.center, sphere.radius);
            }
        }
        #endregion
    } 
}
