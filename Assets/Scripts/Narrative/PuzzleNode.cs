using UnityEngine;
using System.Collections;
using DeceptionPuzzle;

namespace DeceptionNarrative
{
    /// <summary>
    /// A node using a Puzzle to trigger NarrativeNode
    /// </summary>
    [RequireComponent(typeof(NarrativeNode), typeof(Puzzle))]
    public class PuzzleNode : MonoBehaviour
    {
        // Narrative node triggered by this element
        private NarrativeNode narrativeNode = null;

        // Puzzle this node is observing
        private Puzzle puzzle = null;

        // Use this for initialization
        private void Start()
        {
            narrativeNode = GetComponent<NarrativeNode>();
            puzzle = GetComponent<Puzzle>();

            // Check if the narrative node is ready and activate the puzzle if it is
            if(narrativeNode.ArePrerequisitesTriggered())
            {
                ActivatePuzzle();
            }
            // else listen for the ready state
            {
                narrativeNode.OnNodeCanBeTriggered += ActivatePuzzle;
            }
        }

        // Activate the observer puzzle and subscribe to events
        private void ActivatePuzzle()
        {
            puzzle.Activate();
            puzzle.OnPuzzleSolved += PuzzleSolved;
        }

        // Called when the observed puzzle has been solved
        private void PuzzleSolved()
        {
            narrativeNode.TriggerActivated();
        }
    } 
}
