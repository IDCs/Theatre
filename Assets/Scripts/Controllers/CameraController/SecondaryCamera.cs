using UnityEngine;
using System.Collections;

public class SecondaryCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;
    }
}
