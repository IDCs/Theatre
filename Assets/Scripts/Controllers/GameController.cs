using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Top level game control
/// Issues events for UI to subscribe to
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController Controller { get; private set; }
    
    public System.Action<string> OnSubtitlesEvent { get; set; } // Called every time a narrative event passes subtitles to be displayed

    private void Awake()
    {
        if(Controller == null)
        {
            Controller = this;
            DontDestroyOnLoad(Controller);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }


    private void Update()
    {

    }

    public void SubtitlesToDisplay(string text)
    {
        if(OnSubtitlesEvent != null)
        {
            OnSubtitlesEvent(text);
        }
    }
}
