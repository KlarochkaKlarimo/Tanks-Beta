﻿using UnityEditor;

namespace Rowlan.Yapp
{
    public class Constants
    {
        public const string TemplateCollection_FileName = "Prefab Template Collection";
        public const string TemplateCollection_MenuName = "Yapp/Templates/Prefabs/Collection";
        
        public const string PrefabSettingsTemplate_FileName = "Prefab Settings";
        public const string PrefabSettingsTemplate_MenuName = "Yapp/Templates/Prefabs/Settings";

        /// <summary>
        /// Number of prefab template drop targets per row in the template grid of the inspector
        /// </summary>
        public const int PrefabTemplateGridColumnCount = 4;

        /// <summary>
        /// The id for YAPP in Vegetation Studio Pro. Used for the persistent storage.
        /// </summary>
        public const byte VegetationStudioPro_SourceId = 19;

        /// <summary>
        /// The template name which will be used for the tree settings when the trees are extracted from the unity terrain.
        /// </summary>
        public const string TEMPLATE_NAME_TREE = "Tree";
    }
}