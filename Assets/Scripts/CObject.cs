using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Theatre;

[ RequireComponent( typeof( Rigidbody ) ) ]
public class CObject : MonoBehaviour {

    // A set of attributes which any item can posses.
    public enum EObjectAttributes
    {
        ATTRIBUTE_COLLECTABLE,
        ATTRIBUTE_DRAGABLE,
        ATTRIBUTE_OBJECT_OF_INTEREST,
        ATTRIBUTE_IS_KEY,
        ATTRIBUTE_NEEDS_KEY,
    }

    // By default each object will be set with the collectable and dragable attributes
    //  the collectable attribute may be removed if the item's weight is larger than a
    //  constant amount.
    private List< EObjectAttributes > m_liObjectAttributes = new List< EObjectAttributes >
    {
        EObjectAttributes.ATTRIBUTE_COLLECTABLE,
        EObjectAttributes.ATTRIBUTE_DRAGABLE,
    };
    public List< EObjectAttributes > Attributes { get { return m_liObjectAttributes; } }

	private static int m_iAvailableObjectID = 0;

    [ SerializeField ]
    private int m_iObjectID = ++m_iAvailableObjectID;
    public int ClipId { get { return m_iObjectID; } private set { m_iObjectID = value; } }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        // For error reporting.
        string strFunction = "CObject::Start()";

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
        }

        // Check the item's weight.
        if ( rigidbody.mass > Constants.DEFAULT_MAXIMUM_DRAG_WEIGHT )
        {
            // The item is too heavy, remove the dragable attribute.
            m_liObjectAttributes.Remove( EObjectAttributes.ATTRIBUTE_DRAGABLE );
        }
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
}
