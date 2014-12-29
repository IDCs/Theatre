using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CObjectSO : ScriptableObject
{
    [ SerializeField ]
    private GameObject[] m_rggoObjects;

    List< GameObject > m_liObjects;
    public List< GameObject > Objects 
    { 
        get 
        { 
            UpdateObjects(); 
            return m_liObjects; 
        } 
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               UpdateObjects
    /////////////////////////////////////////////////////////////////////////////
    public void UpdateObjects()
    {
        // Clear the audio objects list.
        m_liObjects = new List< GameObject >();

        // Add stuff to it.
        for ( int i = 0; i < m_rggoObjects.Length; ++i )
        {
            m_liObjects.Add( m_rggoObjects[ i ] );
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               FindObject
    /////////////////////////////////////////////////////////////////////////////
    public GameObject FindObject( InventoryItemInfo itemInfo )
    {
        UpdateObjects();

        foreach ( GameObject goObject in m_liObjects )
        {
            CObject cObject = goObject.GetComponent< CObject >();
            if ( cObject.ObjectId == itemInfo.m_iObjectId )
                return goObject;
        }

        return null;
    }
}
