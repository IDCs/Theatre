using UnityEngine;
using System.Collections;

public class ClueControl : MonoBehaviour
{
    [SerializeField]
    private Material hiddenMaterial = null;
    [SerializeField]
    private Material revealedMaterial = null;

    [SerializeField]
    private Light clueLight = null;
    
    [SerializeField]
    private GameObject distortion = null;

    private void Start()
    {
        DetectiveMode.DetectiveModeStatic.OnDetectiveModeOn += Reveal;
        DetectiveMode.DetectiveModeStatic.OnDetectiveModeOff += Hide;
        Hide();
    }

    private void Reveal()
    {
        GetComponent<Renderer>().sharedMaterial = revealedMaterial;
        clueLight.gameObject.SetActive(true);
        distortion.SetActive(false);
    }

    private void Hide()
    {
        GetComponent<Renderer>().sharedMaterial = hiddenMaterial;
        clueLight.gameObject.SetActive(false);
        distortion.SetActive(true);
    }
}
