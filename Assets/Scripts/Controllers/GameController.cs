using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Theatre;

/// <summary>
/// Top level game control
/// Issues events for UI to subscribe to
/// </summary>
public class GameController : MonoBehaviour
{
    private static CObjectSO m_ObjectResource = null;
    public static CObjectSO ObjectResource { get { return m_ObjectResource; } set { m_ObjectResource = value; } }

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
        // For error reporting purposes.
        string strFunction = "GameController::Start()";

        if ( null == m_ObjectResource )
        {
            m_ObjectResource = Resources.Load< CObjectSO >( ResourcePacks.RESOURCE_CONTAINER_OBJECTS );
            if ( null == m_ObjectResource )
            {
                Debug.LogError( string.Format("{0} {1}" + ErrorStrings.ERROR_RESOURCE_PACK, strFunction, ResourcePacks.RESOURCE_CONTAINER_OBJECTS ) );
                return;
            }
        }
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
