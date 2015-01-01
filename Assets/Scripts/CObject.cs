using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Theatre;

[ RequireComponent( typeof( Rigidbody ) ) ]
public class CObject : MonoBehaviour {

    // A static container which holds the item description against its name.
    private static Dictionary< string, string > m_dictDescriptionMap = new Dictionary< string, string >
    {
        { Names.NAME_CUBE, ItemDescriptions.ITEM_DESC_CUBE },
    };
    public static Dictionary< string, string > ItemDescriptionMap { get { return m_dictDescriptionMap; } }

    // A static container holding all active objects.
    private static List< GameObject > m_liActiveObjects = new List< GameObject >();
    public static List< GameObject > ActiveObjects { get { return m_liActiveObjects; } }

    // A set of attributes which any item can posses.
    public enum EObjectAttributes
    {
        ATTRIBUTE_COLLECTABLE,
        ATTRIBUTE_DRAGABLE,
        ATTRIBUTE_OBJECT_OF_INTEREST,
        ATTRIBUTE_IS_KEY,
        ATTRIBUTE_NEEDS_KEY,
    }

    // We're going to use this prefab reference to create the InventoryItemInfo object which
    //  will allow us to hold a reference to this object even when it is stored in the character's
    //  inventory.
    [ SerializeField ]
    private GameObject m_goItemPrefab = null;

    // Will hold the inventory information, keep in mind that this object will remain null if
    //  the collectable attribute isn't present.
    private InventoryItemInfo m_cInventoryItemInfo = null;
    public InventoryItemInfo InvItemInfo { get { return m_cInventoryItemInfo; } }

    // By default each object will be set with the collectable and dragable attributes
    //  the collectable attribute may be removed if the item's weight is larger than a
    //  constant amount.
    private List< EObjectAttributes > m_liObjectAttributes = new List< EObjectAttributes >
    {
        EObjectAttributes.ATTRIBUTE_COLLECTABLE,
        EObjectAttributes.ATTRIBUTE_DRAGABLE,
    };
    public List< EObjectAttributes > Attributes { get { return m_liObjectAttributes; } }

    [ SerializeField ]
    private int m_iObjectID = Constants.DEFAULT_INVALID_OBJECT_ID;
    public int ObjectId { get { return m_iObjectID; } }

    // The developer needs to assign a 2D image to the item.
    [ SerializeField ]
    private Sprite m_spItemSprite = null;

    // Object Events.
    public System.Action< GameObject > OnObjectDragged { get; set; }
    public System.Action< InventoryItemInfo > OnObjectCollected { get; set; }
    public System.Action< InventoryItemInfo > OnObjectDropped { get; set; }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        // For error reporting.
        string strFunction = "CObject::Start()";

        // We're going to run a series of checks to ensure that the object is correctly
        //  configured. This checks include:
        //  1. Ensure that the object Id has been set in the inspector by the dev.
        //  2. Ensure that a rigidbody component is present.
        //  3. Check the item's weight and modify the object's attributes accordingly.
        //  4. Ensure that the object has a reference to the item's prefab for instantiation.
        //  5. Inform the developer if he forgot to assign a 2D image for the UI.

        // Will indicate if this item needs an inventory information object.
        bool bIsCollectable = true;

        // Check if the object Id has been assigned and complain if it hasn't.
        if ( Constants.DEFAULT_INVALID_OBJECT_ID == m_iObjectID )
        {
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_INVALID_OBJECT_ID, m_iObjectID ) );
            return;
        }

        // We need to check the weight of the item and decide if we want to
        //  remove the collectable attribute.
        if ( null == rigidbody )
        {
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_MISSING_COMPONENT, typeof( Rigidbody ).ToString() ) );
            return;
        }

        // Check the item's weight.
        if ( rigidbody.mass > Constants.DEFAULT_HEAVY_WEIGHT )
        {
            // The item is too heavy, remove the collectable attribute.
            m_liObjectAttributes.Remove( EObjectAttributes.ATTRIBUTE_COLLECTABLE );
            bIsCollectable = false;
        }

        // Check the item's weight.
        if ( rigidbody.mass > Constants.DEFAULT_MAXIMUM_DRAG_WEIGHT )
        {
            // The item is too heavy, remove the dragable attribute.
            m_liObjectAttributes.Remove( EObjectAttributes.ATTRIBUTE_DRAGABLE );
        }

        // Check if we assigned an image.
        if ( null == m_spItemSprite )
        {
            Debug.LogWarning( string.Format( "", strFunction, WarningStrings.WARNING_UNASSIGNED_VARIABLE, "m_imgItem" ) );
        }

        // We want to create this item's inventory item info object.
        if ( true == bIsCollectable )
        {
            // Ensure that this object has a reference to a prefab.
            if ( null == m_goItemPrefab )
            {
                // The item prefab is not present, we need to complain and return.
                Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_NULL_OBJECT, "m_goItemPrefab" ) );
                return;
            }

            // Initiate the inventory item.
            m_cInventoryItemInfo = new InventoryItemInfo{ m_iObjectId = this.m_iObjectID, 
                                                          m_strDescription = m_dictDescriptionMap[ gameObject.name ], 
                                                          m_strName = gameObject.name, 
                                                          m_spSprite = this.m_spItemSprite };
        }

        // Add item to the active list.
        m_liActiveObjects.Add( gameObject );
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
    void Update()
    {
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               RunObjectLogic
    /////////////////////////////////////////////////////////////////////////////
    void RunObjectLogic()
    {
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ObjectEventDragged
    /////////////////////////////////////////////////////////////////////////////
    public void ObjectEventDragged( GameObject goObject )
    {
        if( null != OnObjectDragged )
        {
            OnObjectDragged( goObject );
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ObjectEventCollected
    /////////////////////////////////////////////////////////////////////////////
    public void ObjectEventCollected( InventoryItemInfo itemInfo )
    {
        if ( null != OnObjectCollected )
        {
            OnObjectCollected( itemInfo );
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ObjectEventDropped
    /////////////////////////////////////////////////////////////////////////////
    public void ObjectEventDropped( InventoryItemInfo itemInfo )
    {
        if ( null != OnObjectDropped )
        {
            OnObjectDropped( itemInfo );
        }
    }
}

public class InventoryItemInfo
{
    // We're going to use objectIds to pair keys to doors.
    public int m_iObjectId;

    // Will hold the item's name.
    public string m_strName;

    // Will hold the item's description.
    public string m_strDescription;

    // The item sprite for the UI.
    public Sprite m_spSprite;
}
