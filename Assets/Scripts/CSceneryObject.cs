using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Theatre;

public class CSceneryObject : MonoBehaviour {

    private static List< GameObject > m_liSceneryObjects = new List< GameObject >();
    public static List<GameObject> SceneryObjects { get { return m_liSceneryObjects; } }

	// Use this for initialization
	void Start () 
    {
        string strFunction = "CSceneryController::Start()";

        if ( gameObject.tag == Tags.TAG_SCENERY )
	        m_liSceneryObjects.Add( gameObject );
        else
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_SCENERY_TAG_NOT_FOUND, gameObject.name ) );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
