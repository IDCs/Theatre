﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Theatre;

public class CInventory : MonoBehaviour {

    public enum EInventoryState
    {
        STATE_NONE,
        STATE_IN,
        STATE_OUT,
        STATE_SLIDEIN,
        STATE_SLIDEOUT,
    }

    private static CInventory m_cInventoryInstance;
    public static CInventory InventoryInstance
    {
        get
        {
            if ( null == m_cInventoryInstance )
            {
                m_cInventoryInstance = GameObject.FindObjectOfType< CInventory >();

                // Ensure we don't destroy on load.
                DontDestroyOnLoad( m_cInventoryInstance.gameObject );
            }

            return m_cInventoryInstance;
        }
    }

    [ SerializeField ]
    private GameObject m_goSlotPrefab = null;

    [ SerializeField ]
    private static List< GameObject > m_liInventorySlots = new List< GameObject >();
    public static List<GameObject> InventorySlots { get { return m_liInventorySlots; } }

    private static Dictionary< int, EInventoryState > m_dictHashStates = new Dictionary< int, EInventoryState >
    {
        { Animator.StringToHash( "InventoryAnimator.In" ), EInventoryState.STATE_IN },
        { Animator.StringToHash( "InventoryAnimator.SlideIn" ), EInventoryState.STATE_SLIDEIN },
        { Animator.StringToHash( "InventoryAnimator.SlideOut" ), EInventoryState.STATE_SLIDEOUT },
        { Animator.StringToHash( "InventoryAnimator.Out" ), EInventoryState.STATE_OUT },
    };

    private Animator m_anAnimator = null;

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Awake
    /////////////////////////////////////////////////////////////////////////////
    void Awake ()
    {
        if ( null == m_cInventoryInstance )
        {
            // This is the first singleton and no other reference has been found.
            m_cInventoryInstance = this;
            DontDestroyOnLoad( this );
        }
        else
        {
            // Singleton already exists, check if it's another reference and destroy
            //  this gameobject if it is.
            if ( this != m_cInventoryInstance )
                Destroy( this.gameObject );
        }
    }

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        // Error reporting
        string strFunction = "CInventory::Start()";

        // Ensure that the slot prefab has been assigned.
        if ( null == m_goSlotPrefab )
        {
            // Slot prefab is unassigned, complain and return.
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_NULL_OBJECT, "m_goSlotPrefab" ) );
            return;
        }

        // Get a handle on the Animator component attached to the inventory panel.
        m_anAnimator = GetComponent< Animator >();
        if ( null == m_anAnimator )
        {
            // Animator is missing, inform the dev.
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_MISSING_COMPONENT, typeof( Animator ).ToString() ) );
        }

        // Get the size of the panel itself so we can evenly distribute the slots 
        //  according to panel size.
        Rect rcPanelSize = this.GetComponent< RectTransform >().rect;

        // Calculate rectangle offset.
        float fWidth = rcPanelSize.width;
        float fHeight = rcPanelSize.height;
        float fSlotWidth = m_goSlotPrefab.GetComponent< RectTransform >().rect.width;

        // Find the X spacing between each slot.
        float fXSpacing = ( ( fWidth / Constants.DEFAULT_MAX_INVENTORY_SLOTS ) - fSlotWidth ) / 2;

        // Find the panel's center.
        float fYCenter = fHeight / 2;
        float fXCenter = fWidth / 2;
        
        // Find the initial slot offset.
        float fXOffset = -fXCenter + ( fSlotWidth / 2 ) + fXSpacing;

        // Generate all the inventory slots.
	    for ( int i = 0; i < Constants.DEFAULT_MAX_INVENTORY_SLOTS; ++i )
        {
            GameObject goSlot = Instantiate( m_goSlotPrefab ) as GameObject;
            if ( null == goSlot )
            {
                // We did not manage to instantiate the slot, this should never happen.
                Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_NULL_OBJECT, "goSlot" ) );
                return;
            }

            goSlot.transform.parent = this.transform;

            // Apply X offset.
            goSlot.GetComponent< RectTransform >().localPosition = new Vector3( fXOffset, 0f, 0f );

            // Jump to the next ideal position.
            fXOffset += fWidth / Constants.DEFAULT_MAX_INVENTORY_SLOTS;

            // Keep a list of all the slots we created.
            m_liInventorySlots.Add( goSlot );
        }
	}
	
	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update () 
    {
        // The Animator is missing, there's no point in going forward.
        if ( null == m_anAnimator )
            return;

        // Check for user input.
	    if ( true == Input.GetKeyDown( KeyCode.I ) )
        {
            // Get the animator state info object. We're going to use the namehash to find
            //  out which animator state we're in.
            AnimatorStateInfo animatorStateInfo = m_anAnimator.GetCurrentAnimatorStateInfo(0);
            EInventoryState eState = GetInventoryState( animatorStateInfo.nameHash );

            // Will dictate if we can transition or not.
            bool bCanTransition = false;
            
            switch ( eState )
            {
                case EInventoryState.STATE_IN:
                case EInventoryState.STATE_OUT:

                    bCanTransition = true;

                    break;

                default:

                    bCanTransition = false;

                    break;
            }

            if ( true == bCanTransition )
                m_anAnimator.SetTrigger( AnimatorValues.ANIMATOR_TRIGGER_SHOW_INVENTORY );
        }
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               GetInventoryState
    /////////////////////////////////////////////////////////////////////////////
    public EInventoryState GetInventoryState( int iNameHash )
    {
        if ( m_dictHashStates.ContainsKey( iNameHash ) )
            return m_dictHashStates[ iNameHash ];

        return EInventoryState.STATE_NONE;
    }
}
