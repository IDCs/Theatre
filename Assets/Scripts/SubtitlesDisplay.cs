using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SubtitlesDisplay : MonoBehaviour
{
    private Text textComponent = null;
    private GameController gameController = null;

    private void Start()
    {
        textComponent = GetComponent<Text>();

        // Access the game controller and start listening for subtitle events
        gameController = GameController.Controller;
        if(gameController != null)
        {
            gameController.OnSubtitlesEvent += DisplaySubtitles;
        }
    }

    private void DisplaySubtitles(string text)
    {
        textComponent.text = text;
    }

    private void OnDisable()
    {
        if (gameController != null)
        {
            gameController.OnSubtitlesEvent -= DisplaySubtitles;
        }
    }
}
