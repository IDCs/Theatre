﻿using UnityEngine;
using System.Collections;

namespace Theatre
{ 
    public class Constants 
    {
        public const string GAME_THEATRE = "Theatre";
        
        public const float DEFAULT_HEAVY_WEIGHT = 10.0f;
        public const float DEFAULT_MAXIMUM_DRAG_WEIGHT = 50.0f;
        public const int DEFAULT_INVALID_OBJECT_ID = -666;

        public const int DEFAULT_MAX_INVENTORY_SLOTS = 10;
    }

    public class DefaultPaths
    {
        // Editor Custom Menu Items paths.
        // Will be used by the custom menu items script for resource creation.
        public const string SO_RESOURCES_PATH = "Theatre/Resources/";
        public const string SO_AUDIO = "Audio";
        public const string SO_SUBTITLED_AUDIO = "Subtitled Audio";
        public const string SO_OBJECTS = "Objects";
        
        // Asset paths
        public const string PATH_RESOURCES = "Resources/";

        // Default audio path
        public const string PATH_AUDIO = "./Assets/Audio";
    }

    public class Controls
    {
        // Control related constants
        public const string CONTROL_AXIS_HORIZONTAL = "Horizontal";
        public const string CONTROL_AXIS_VERTICAL = "Vertical";
        public const string CONTROL_MOUSE_X = "Mouse X";
        public const string CONTROL_MOUSE_Y = "Mouse Y";

        public const int CONTROL_MOUSE_LEFT_BUTTON = 0;
        public const int CONTROL_MOUSE_RIGHT_BUTTON = 1;
    }

    public class Audio
    {
        // Default regex patterns.
        public const string AUDIO_MUSIC = "Music";
        public const string AUDIO_EFFECT_GAMEOVER = "LevelFailed";
        public const string AUDIO_EFFECT_MENU_SELECT = "Menu_Select";
        public const string AUDIO_EFFECT_LEVEL_COMPLETED = "LevelCompleted";

        // Valid file extensions.
        public const string FILE_TYPE_MP3 = ".mp3";
        public const string FILE_TYPE_WAV = ".wav";

        // Audio altering variables.
        public const float AUDIO_FADE_VARIABLE = 0.3f;
    }

    public class Tags
    {
        // List of commonly used tags.
        public const string TAG_PLAYER = "Player";
        public const string TAG_WALL = "Wall";
        public const string TAG_AUDIO_CONTROLLER = "AudioController";
        public const string TAG_AUDIO = "Audio";
        public const string TAG_SCENERY = "Scenery";
    }

    public class Names
    {
        // List of commonly used names.
        public const string NAME_HELD_OBJECT = "HeldObject";
        public const string NAME_CUBE = "Cube";
    }

    public class ErrorStrings
    {
        // Declare error messages.
        public const string ERROR_UNHANDLED_DEVICE = "is an unsupported device type";
        public const string ERROR_UNRECOGNIZED_VALUE = "Provided enum value is unrecognized";
        public const string ERROR_NULL_OBJECT = "Failed to find object";
        public const string ERROR_MISSING_COMPONENT = "Missing component";
        public const string ERROR_UNRECOGNIZED_NAME = "Provided name is not handled by current function.";
        public const string ERROR_UNMATCHED_AUDIO_CLIP = "Unable to match provided audio file to available patterns.";
        public const string ERROR_AUDIO_FILES_NOT_LOADED = "Audio Controller has indicated that it hasn't finished loading all audio files.";
        public const string ERROR_AUDIO_FAILED_RELOAD = "Could not load audio resources.";
        public const string ERROR_SCENERY_TAG_NOT_FOUND = "SceneryController is attached to an incorrectly tagged object.";
        public const string ERROR_INVALID_OBJECT_ID = "Invalid object id.";
        public const string ERROR_RESOURCE_PACK = "Resource pack could not be loaded.";
    }

    public class AnimatorValues
    {
        
        // Inventory animator values
        public const string ANIMATOR_TRIGGER_SHOW_INVENTORY = "triggerInventory";
    }

    public class WarningStrings
    {
        // Declare warning messages.
        public const string WARNING_UNASSIGNED_VARIABLE = "The following variable has not been assigned in the inspector.";
    }

    public class ItemDescriptions
    {
        // Declare item descriptions here.
        public const string ITEM_DESC_CUBE = "It's an amazing cube!";
    }

    public class ResourcePacks
    {
        public const string RESOURCE_CONTAINER_AUDIO_OBJECTS = "AudioResource";
        public const string RESOURCE_CONTAINER_OBJECTS = "Objects";
    }
}