using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectiveMode : MonoBehaviour
{
    public static DetectiveMode DetectiveModeStatic { get; private set; }

    public System.Action OnDetectiveModeOn { get; set; }
    public System.Action OnDetectiveModeOff { get; set; }

    private bool inDetectiveMode = false;

    private List<MonoBehaviour> detectiveModeEffects = new List<MonoBehaviour>();

    void Awake()
    {
        DetectiveModeStatic = this;
        detectiveModeEffects.Add(GetComponentInChildren<VortexEffect>());
        detectiveModeEffects.Add(GetComponentInChildren<ScreenOverlay>());
        detectiveModeEffects.Add(GetComponentInChildren<Vignetting>());


        DisableDetectiveMode();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            inDetectiveMode = !inDetectiveMode;
            if (inDetectiveMode)
            {
                EnableDetectiveMode(); 
            }
            else if (!inDetectiveMode)
            {
                DisableDetectiveMode();
            }
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void EnableDetectiveMode()
    {
        if (OnDetectiveModeOn != null)
        {
            OnDetectiveModeOn();
        }

        foreach (MonoBehaviour effect in detectiveModeEffects)
        {
            effect.enabled = true;
        }
    }

    void DisableDetectiveMode()
    {
        if (OnDetectiveModeOff != null)
        {
            OnDetectiveModeOff();
        }

        foreach (MonoBehaviour effect in detectiveModeEffects)
        {
            effect.enabled = false;
        }
    }
}
