using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Theatre;

public class CSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {

    public enum ESlotState
    {
        STATE_NONE,
        STATE_EMPTY,
        STATE_TAKEN,
    }

    // Will hold the item information object, we can use this
    //  to retrieve the name of the image and instantiate it if we have to.
    private InventoryItemInfo m_cItemInfo = null;

    // The 2D image of the item which we're going to display on the UI.
    private Image m_imgItem;

    // The sprite.
    private Sprite m_spItemSprite;

    // The slot state will dictate if we can store or drop items.
    [ SerializeField ]
    private ESlotState m_eSlotState = ESlotState.STATE_EMPTY;
    public ESlotState SlotState { get { return m_eSlotState; } }

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
        // Error reporting.
        string strFunction = "CSlot::OnPointerDown()";

        // Following logic should only execute when the slot state is set to "Taken"
        //  therefore we assume that the inventoryiteminfo object has been set.
        if ( m_eSlotState != ESlotState.STATE_TAKEN )
            return;

        // Get the position where we want to instantiate the object.
        Vector3 v3HandsPosition = CCameraController.HandsPosition;

        GameObject goPrefab = GameController.ObjectResource.FindObject( m_cItemInfo );
        if ( null == goPrefab )
        {
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_NULL_OBJECT, "goPrefab" ) );
            return;
        }

        // Instantiate the item.
        GameObject goObject = Instantiate( goPrefab, v3HandsPosition, Quaternion.identity ) as GameObject;
        goObject.name = m_cItemInfo.m_strName;

        // Indicate that the slot is now empty.
        m_eSlotState = ESlotState.STATE_EMPTY;
        m_cItemInfo = null;
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnPointerEnter
    /////////////////////////////////////////////////////////////////////////////
    public void OnPointerEnter( PointerEventData pointerEventData )
    {
        Debug.Log("PointerEnter");
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               AddItemInfo
    /////////////////////////////////////////////////////////////////////////////
    public void AddItemInfo( InventoryItemInfo invItemInfo )
    {
        // This function assumes that the inventory has checked this slot prior to 
        //  calling the function, and has detected that this slot is empty.

        // Set the slot to the taken state.
        m_eSlotState = ESlotState.STATE_TAKEN;

        // Set the itemInfo object.
        m_cItemInfo = invItemInfo;
    }
}
