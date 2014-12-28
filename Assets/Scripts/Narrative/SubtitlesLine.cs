using UnityEngine;
using System.Collections;

namespace DeceptionNarrative
{
    [System.Serializable]
    public class SubtitlesLine
    {
        [SerializeField]
        private float time;
        public float Time { get { return time; } }

        [SerializeField]
        private string text;
        public string Text { get { return text; } }
    } 
}
