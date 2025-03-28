﻿//Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using TheVisualEngine;

public class TVEShaderGUIHelper : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        var material0 = materialEditor.target as Material;
        var materials = materialEditor.targets;

        DrawDynamicInspector(material0, materialEditor, props);
    }

    void DrawDynamicInspector(Material material, MaterialEditor materialEditor, MaterialProperty[] props)
    {
        var customPropsList = new List<MaterialProperty>();

        for (int i = 0; i < props.Length; i++)
        {
            var prop = props[i];

            if (prop.flags == MaterialProperty.PropFlags.HideInInspector)
            {
                continue;
            }

            if (prop.name == "unity_Lightmaps")
                continue;

            if (prop.name == "unity_LightmapsInd")
                continue;

            if (prop.name == "unity_ShadowMasks")
                continue;

            if (prop.name.Contains("_Banner"))
                continue;

            customPropsList.Add(prop);
        }

        //Draw Custom GUI
        for (int i = 0; i < customPropsList.Count; i++)
        {
            var prop = customPropsList[i];

            materialEditor.ShaderProperty(prop, prop.displayName);

        }

        GUILayout.Space(20);

        TheVisualEngine.TVEUtils.DrawPoweredBy();
    }
}

