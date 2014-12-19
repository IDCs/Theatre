using UnityEngine;
using System.Collections;

public class lightControl : MonoBehaviour
{
    [SerializeField]
    private Light[] lightsToIgnore = null;

    void OnPreCull()
    {
        foreach (Light light in lightsToIgnore)
        {
            light.enabled = false;
        }
    }

    void OnPostRender()
    {
        foreach (Light light in lightsToIgnore)
        {
            light.enabled = true;
        }
    }
}
