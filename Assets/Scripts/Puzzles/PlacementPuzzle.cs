using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace DeceptionPuzzle
{
    /// <summary>
    /// Puzzele controller for puzzles depending on objects being placed in correct ObjectSlots
    /// </summary>
    public class PlacementPuzzle : Puzzle
    {
        // All the slots that have to have correct objects attached for the puzzle to be solved
        [SerializeField]
        private ObjectSlot[] slots = null;

        public override void Activate()
        {
            base.Activate();

            // Activate all stolts and start listening for objects being attached
            foreach (ObjectSlot slot in slots)
            {
                slot.Activate();
                slot.OnReceivedObject += OnSlotReceivedObject;
            }
        }

        private void OnSlotReceivedObject(bool isCorrectObject)
        {
            // If the slot received a correct object check if this solved the puzzle
            if (isCorrectObject)
            {
                if (slots.All(s => s.HasCorrectObject()))
                {
                    PuzzleSolved();
                }
            }
        }

        protected override void PuzzleSolved()
        {
            base.PuzzleSolved();
            // Deactivate all slots
            foreach (ObjectSlot slot in slots)
            {
                slot.Deactivate();
                slot.OnReceivedObject -= OnSlotReceivedObject;
            }
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (ObjectSlot slot in slots)
            {
                Gizmos.DrawLine(slot.transform.position, transform.position);
            }
        }
        #endregion
    }
    
}