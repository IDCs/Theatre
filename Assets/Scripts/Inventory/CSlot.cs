using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Theatre;

public class CSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

    // Will hold the item information object, we can use this
    //  to retrieve the name of the image and instantiate it if we have to.
    private InventoryItemInfo m_cItemInfo = null;

    // The 2D image of the item which we're going to display on the UI.
    private Image m_imgItem;

    // The sprite.
    private Sprite m_spItemSprite;

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        // Error reporting.
        string strFunction = "CSlot::Start()"; 

        // Attempt to get a handle on the image component of this slot.
	    m_imgItem = GetComponent< Image >();
        if ( null == m_imgItem )
        {
            // The image component is missing, inform the user.
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_MISSING_COMPONENT, typeof( Image ).ToString() ) );
        }
	}
	
	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update () 
    {
        // Item isn't assigned, we don't have to do anything.
	    if ( null == m_cItemInfo )
            return;

        // Get the sprite from the item.
        m_spItemSprite = m_cItemInfo.m_spSprite;

        // We want to display the image.
        if ( null != m_imgItem )
        {
            m_imgItem.enabled = true;
            m_imgItem.sprite = m_spItemSprite;
        }
        else
        {
            m_imgItem.enabled = false;
        }
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnPointerDown
    /////////////////////////////////////////////////////////////////////////////
    public void OnPointerDown( PointerEventData pointerEventData )
    {
        Debug.Log("PointerDown");
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnPointerEnter
    /////////////////////////////////////////////////////////////////////////////
    public void OnPointerEnter( PointerEventData pointerEventData )
    {
        Debug.Log("PointerEnter");
    }
}
