using UnityEngine;
using UnityEditor;
using System.Collections;
using Theatre;

public static class CustomMenuItems
{
    [ MenuItem( DefaultPaths.SO_RESOURCES_PATH + DefaultPaths.SO_AUDIO ) ]
    public static void CreateAudioSO()
    {
        ScriptableObjectUtility.CreateResource< CAudioSO >( ResourcePacks.RESOURCE_CONTAINER_AUDIO_OBJECTS );
    }

    [MenuItem(DefaultPaths.SO_RESOURCES_PATH + DefaultPaths.SO_SUBTITLED_AUDIO)]
    public static void CreateSubtitledAudio()
    {
        ScriptableObjectUtility.CreateResource<DeceptionNarrative.SubtitledAudio>();
    }

    [ MenuItem( DefaultPaths.SO_RESOURCES_PATH + DefaultPaths.SO_OBJECTS ) ]
    public static void CreateObjectSO()
    {
        ScriptableObjectUtility.CreateResource< CObjectSO >( ResourcePacks.RESOURCE_CONTAINER_OBJECTS );
    }
}
