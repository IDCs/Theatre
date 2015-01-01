using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Theatre;

public class CCameraController : MouseLook {

    // Will hold the default position for all dragged objects.
    private static GameObject m_goDraggedObject = null;
    public static Vector3 HandsPosition 
    { 
        get 
        { 
            if ( null == m_goDraggedObject )
            {
                // This should never happen.
                Debug.LogError( string.Format( "CCameraController::HandsPosition {0}: {1}", ErrorStrings.ERROR_NULL_OBJECT, "m_goDraggedObject" ) );
                return Vector3.zero;
            }

            return m_goDraggedObject.transform.position; 
        } 
    }

    // Will hold a reference to the object which we're currently looking at.
    private GameObject m_goInteractableObject = null;

    // Will hold a reference to the object which we're holding ( if any ).
    private GameObject m_goHeldObject = null;

    // Will indicate if we want the script to run its drag logic.
    [ SerializeField ]
    private bool m_bRunDragLogic = true;
    public bool DragEnabled { get { return m_bRunDragLogic; } }

    // Will indicate if the camera is allowed to rotate
    [ SerializeField ]
    private bool m_bAllowRotation = true;
    public bool AllowRotation { get { return m_bAllowRotation; } }

    private List< GameObject > m_liSceneryObjects = new List< GameObject >();

    private float m_fClickTime = 0;

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
    void Start ()
    {
        // For error reporting.
        string strFunction = "CCameraController::Start()";

        // Run the base class's start function.
        base.Start();

        // Get the held object gameobject.
        foreach ( Transform trObject in gameObject.GetComponentsInChildren< Transform >() )
        {
            if ( trObject.name == Names.NAME_HELD_OBJECT )
            {
                m_goDraggedObject = trObject.gameObject;
                break;
            }
        }

        // Check if we managed to find the held game object.
        if ( null == m_goDraggedObject )
        {
            // We didn't manage to find the gameobject.
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_NULL_OBJECT, "DraggedObject" ) );
            
            // Flag that we don't want to enable drag logic for this Camera Controller.
            m_bRunDragLogic = false;

            return;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update ()
	{
        if ( m_liSceneryObjects.Count == 0 )
            m_liSceneryObjects = CSceneryObject.SceneryObjects;

        // Check if we're allowed to run the camera rotation.
        if ( true == m_bAllowRotation )
        { 
            // Run the base class's update function.
            base.Update();

            // Run the grip logic.
            SearchForInteractables();
        }
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               FixedUpdate
    /////////////////////////////////////////////////////////////////////////////
    void FixedUpdate ()
    {
        // Run logic that reacts to mouse events.
        //  This may include physics manipulation, so we want to run this within
        //  the fixed update function.
        CheckForMouseInput();

        // Run logic that reacts to keyboard events.
        CheckForKeyboardInput();
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               CheckForMouseInput
    /////////////////////////////////////////////////////////////////////////////
    private void CheckForMouseInput ()
    {
        // Check for mouse input and react accordingly.
        if ( Input.GetMouseButtonDown( Controls.CONTROL_MOUSE_LEFT_BUTTON ) )
        {
            // A click has occured, depending on 
            m_fClickTime = Time.time;

            // Drag the interactable object.
            DragObject();
        }
        else if ( Input.GetMouseButtonUp( Controls.CONTROL_MOUSE_LEFT_BUTTON ) )
        {
            // We're not holding anything, return.
            if ( null == m_goHeldObject )
                return;

            if ( m_fClickTime + 1f > Time.time )
            {
                // Attempt to add the item to inventory, AddToInventory will return
                //  true or false depending if it succeeded.
                bool bItemAdded = AddToInventory();

                if ( true == bItemAdded )
                { 
                    // We managed to add the item to inventory, clean up and return.
                    Destroy( m_goHeldObject );
                    m_goHeldObject = null;

                    return;
                }
            }

            ReleaseObject();
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               AddToInventory
    /////////////////////////////////////////////////////////////////////////////
    private bool AddToInventory ()
    {
        // Error reporting.
        string strFunction = "CCameraController::AddToInventory()";

        // We need to check the item's attributes and ensure that we can store it
        //  in our inventory. It's safe to assume that we have a held object at this point
        //  so we're not going to null check it.
        CObject cObject = m_goHeldObject.GetComponent< CObject >();
        if ( null == cObject )
        {
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_MISSING_COMPONENT, typeof( CObject ).ToString() ) );
            return false;
        }

        // Check if the item is collectable.
        if ( true == cObject.Attributes.Contains( CObject.EObjectAttributes.ATTRIBUTE_COLLECTABLE ) )
        {
            bool bItemAdded = CInventory.InventoryInstance.AddToInventory( cObject.InvItemInfo );

            if ( true == bItemAdded )
            { 
                // Call the object event.
                cObject.ObjectEventCollected( cObject.InvItemInfo );
            
                return true;
            }
            else
            {
                // TODO: Inform the player that his inventory is full.
            }
        }

        return false;
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               RunRotationControls
    /////////////////////////////////////////////////////////////////////////////
    private void CheckRotationControls ()
    {
        // We want to be able to rotate the held object if the X key is held down.
        if ( true == Input.GetKey( KeyCode.X ) )
        {
            // MouseLook objects interfere with item rotation.
            MouseLook[] rgMouseLookObjects = GetComponentsInChildren< MouseLook >();
            for ( int i = 0; i < rgMouseLookObjects.Length; ++i )
            {
                // Disallow camera rotation.
                rgMouseLookObjects[ i ].AllowRotation = false;
            }

            // Disable collisions for all scenery objects.
            foreach ( GameObject goSceneryObject in m_liSceneryObjects )
            {
                if ( goSceneryObject != m_goHeldObject )
                    Physics.IgnoreCollision( goSceneryObject.collider, m_goHeldObject.collider, true );
            }

            // Disallow camera rotation.
            m_bAllowRotation = false;

            // Get Input from mouse.
            float fHorizontalInput = Input.GetAxis( Controls.CONTROL_MOUSE_X );
            float fVerticalInput = Input.GetAxis( Controls.CONTROL_MOUSE_Y );

            // Create the direction vector and normalize it.
            Vector3 v3Direction = new Vector3( 0, -fHorizontalInput, -fVerticalInput );
            v3Direction.Normalize();

            //rotate us over time according to speed until we are in the required rotation
            m_goHeldObject.transform.Rotate( v3Direction );

        }
        else if ( true == Input.GetKeyUp( KeyCode.X ) )
        {
            m_bAllowRotation = true;

            // We're done rotating, we can re-enable camera rotation.
            MouseLook[] rgMouseLookObjects = GetComponentsInChildren< MouseLook >();
            for ( int i = 0; i < rgMouseLookObjects.Length; ++i )
            {
                rgMouseLookObjects[ i ].AllowRotation = true;
            }

            // Enable collisions for all scenery objects.
            foreach ( GameObject goSceneryObject in m_liSceneryObjects )
            {
                if ( goSceneryObject != m_goHeldObject )
                    Physics.IgnoreCollision( goSceneryObject.collider, m_goHeldObject.collider, false );
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               CheckForKeyboardInput
    /////////////////////////////////////////////////////////////////////////////
    private void CheckForKeyboardInput ()
    {
        // Check if the user is trying to rotate a held object.
        if ( null != m_goHeldObject )
            CheckRotationControls();
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               SearchForInteractables
    /////////////////////////////////////////////////////////////////////////////
    private void SearchForInteractables ()
    {
        // Check if we want the drag logic to run.
        if ( false == m_bRunDragLogic )
            return;

        // We're going to cast a ray going from the main camera to the middle of the screen.
        //  and store whatever we hit inside a raycast hit struct. This will allow us to 
        //  to get a handle on whatever object is in front of the camera.
        Ray sRay = Camera.main.ViewportPointToRay ( new Vector3( 0.5f, 0.5f, 0f ) );
        RaycastHit sRayCastHit;

        // Check if we have a hit.
        if( true == Physics.Raycast( sRay, out sRayCastHit, 10.0f ) )
        {
            // Draw the line in the editor so we can see what we're
            //  hitting and get a handle on the gameobject.
            Debug.DrawLine ( sRay.origin, sRayCastHit.point );
            m_goInteractableObject = sRayCastHit.collider.gameObject;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               DragObject
    /////////////////////////////////////////////////////////////////////////////
    void DragObject ()
    {
        // For error reporting.
        string strFunction = "CCameraController::OnMouseDown()";

        // Check if we have an interactable object in front of us.
        //  Additionally, we don't want to proceed if the camera controller couldn't
        //  find the dragged gameobject.
        if ( null == m_goInteractableObject || false == m_bRunDragLogic || m_goInteractableObject.tag == Tags.TAG_SCENERY )
            return;
        
        // We're now holding an item, keep a reference to it.
        m_goHeldObject = m_goInteractableObject;

        // Get a handle on the object script
        CObject cObject = m_goHeldObject.GetComponent< CObject >();
        if ( null == cObject )
        {
            Debug.LogError( string.Format( "{0} {1}: {2}", strFunction, ErrorStrings.ERROR_MISSING_COMPONENT, typeof( CObject ).ToString() ) );
            return;
        }

        // Call the dragged event.
        cObject.ObjectEventDragged( m_goHeldObject );

        // Retrieve the object's attributes and ensure that we are able to drag
        //  the object.
        List< CObject.EObjectAttributes > liObjectAttributes = cObject.Attributes;
        if ( false == liObjectAttributes.Contains( CObject.EObjectAttributes.ATTRIBUTE_DRAGABLE ) )
        {
            // The item isn't dragable, we can return.
            return;
        }

        // Assign the object's position to the held gameobject position and parent it.
        cObject.transform.position = m_goDraggedObject.transform.position;
        cObject.transform.parent = m_goDraggedObject.transform;

        // Turn off gravity for the object while we're holding it.
        cObject.rigidbody.useGravity = false;
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ReleaseObject
    /////////////////////////////////////////////////////////////////////////////
    void ReleaseObject ()
    {
        // Check if the drag logic is enabled before we proceed
        if ( false == m_bRunDragLogic )
            return;

        // Check if we're actually holding anything and return if we don't
        if ( null == m_goHeldObject )
            return;

        // We want to get the object script of the currently held item in order
        //  to call the dropped object event. We assume that a CObject component
        //  is attached to the gameobject otherwise we wouldn't get this far.
        CObject cObject = m_goHeldObject.GetComponent< CObject >();
        
        // Call the event.
        cObject.ObjectEventDropped( cObject.InvItemInfo );

        // Remove the held object and reactivate physics.
        m_goHeldObject.rigidbody.useGravity = true;
        m_goHeldObject.transform.parent = null;

        m_goHeldObject = null;
    }
}