using UnityEngine;
using System.Collections;
using System;

namespace DeceptionPuzzle
{
    /// <summary>
    /// Abstract base for puzzle controllers
    /// </summary>
    public abstract class Puzzle : MonoBehaviour
    {
        protected enum PuzzleState { PuzzleInactive, PuzzleActive, PuzzleSolved }
        protected PuzzleState StateOfPuzzle { get; set; }

        // Event emited when the puzzle has been solved
        public Action OnPuzzleSolved { get; set; }

        // Initialise the puzzle and its elements
        public virtual void Activate()
        {
            StateOfPuzzle = PuzzleState.PuzzleActive;
        }

        // Actions to take when puzzle is solved - deactivate elements and clean up
        protected virtual void PuzzleSolved()
        {
            StateOfPuzzle = PuzzleState.PuzzleSolved;
            if (OnPuzzleSolved != null)
            {
                OnPuzzleSolved();
            }
        }

        public bool IsPuzzleSolved()
        {
            return StateOfPuzzle == PuzzleState.PuzzleSolved;
        }
    } 
}
