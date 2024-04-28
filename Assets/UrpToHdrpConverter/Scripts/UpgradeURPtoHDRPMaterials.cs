#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Rendering;

namespace UrpTOHdrpConverter
{
    class UrpTOHdrpConverter : MaterialUpgrader
    {
        public UrpTOHdrpConverter(string sourceShaderName, string destShaderName, MaterialFinalizer finalizer = null)
        {
            if (sourceShaderName == null) throw new ArgumentNullException("oldShaderName");

            RenameShader(sourceShaderName, destShaderName, finalizer);
            RenameTexture("_BaseMap", "_BaseColorMap");
            RenameTexture("_BumpMap", "_NormalMap");
            RenameTexture("_MetallicGlossMap", "_MaskMap");
            RenameFloat("_BumpScale", "_NormalScale");
            RenameTexture("_EmissionMap", "_EmissiveColorMap");
            RenameColor("_EmissionColor", "_EmissiveColor");
            RenameFloat("_Cutoff", "_AlphaCutoff");
        }

        public static void ConvertURPtoHDRPMaterialKeywords(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            if (material.GetTexture("_MetallicGlossMap"))
                material.SetFloat("_Smoothness", 1);

            material.SetFloat("_WorkflowMode", 1.0f);
            CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));
            CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
            DowngradeSurfaceTypeAndBlendModeNow(material);
            BaseShaderGUI.SetupMaterialBlendMode(material);
        }

        static void DowngradeSurfaceTypeAndBlendModeNow(Material material)
        {
            if (material.HasProperty("_Surface"))
                material.SetInt("_SurfaceType", (int)material.GetFloat("_Surface"));
            if (material.HasProperty("_Blend"))
                material.SetInt("_BlendMode", (int)material.GetFloat("_Blend"));
        }
    }

    class DowngradeHdrpTOUrpMats
    {
        static List<MaterialUpgrader> GetHDtoURPUpgraders()
        {
            var upgraders = new List<MaterialUpgrader>();
            upgraders.Add(new UrpTOHdrpConverter("Universal Render Pipeline/Lit", "HDRP/Lit", UrpTOHdrpConverter.ConvertURPtoHDRPMaterialKeywords));
            return upgraders;
        }

        [MenuItem("Edit/Render Pipeline/Convert All URP Materials to HDRP Materials", priority = CoreUtils.Priorities.editMenuPriority)]
        internal static void UpgradeMaterialsProject()
        {
            MaterialUpgrader.UpgradeProjectFolder(GetHDtoURPUpgraders(), "Converting Now. Please Wait...");
        }

        [MenuItem("Edit/Render Pipeline/Convert Selected URP Materials to HDRP Materials", priority = CoreUtils.Priorities.editMenuPriority)]
        internal static void UpgradeMaterialsSelection()
        {
            MaterialUpgrader.UpgradeSelection(GetHDtoURPUpgraders(), "Converting Now. Please Wait...");
        }
    }
}
#endif