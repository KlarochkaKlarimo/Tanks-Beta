﻿// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheVisualEngine
{
    [HelpURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.fd5y8rbb7aia")]
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Visual Engine/TVE Element")]
    public class TVEElement : StyledMonoBehaviour
    {
        [StyledBanner(0.890f, 0.745f, 0.309f, "Element")]
        public bool styledBanner;

        [Tooltip("Sets the element visibility.")]
        public TVEElementVisibility customVisibility = TVEElementVisibility.UseGlobalSettings;
        [Tooltip("Sets a custom material for element rendering.")]
        public Material customMaterial;

        [Space(10)]
        [Tooltip("Sets the terrain height or splat map as element textures.")]
        public Terrain terrainData;
        public TVETerrainTexture terrainMask = TVETerrainTexture.Auto;

#if UNITY_EDITOR
        [StyledText(TextAnchor.MiddleRight, 10, 0)]
        public string debugData = "";
#endif

        [HideInInspector]
        public TVEElementMaterialData materialData;
        [System.NonSerialized]
        public Renderer elementRenderer;
        [System.NonSerialized]
        public Material elementMaterial;
        [System.NonSerialized]
        public Mesh elementMesh;
        [System.NonSerialized]
        public Vector4 elementParams = Vector4.one;
        [System.NonSerialized]
        public int elementID = 0;
        [System.NonSerialized]
        public int instancedID = 0;
        [System.NonSerialized]
        public string renderName = "";
        [System.NonSerialized]
        public int renderDataID = 0;
        [System.NonSerialized]
        public List<int> renderLayers;
        [System.NonSerialized]
        public int renderPass = 0;
        [System.NonSerialized]
        public bool isActive = false;

        new ParticleSystem particleSystem;

        int useVertexColorDirection = 0;
        int useRaycastFading = 0;
        Vector3 lastPosition;

        LayerMask raycastMask;
        float raycastEnd = 0;

        float speedTreshold = 10;

        bool isSelected = false;

        void OnEnable()
        {
            particleSystem = gameObject.GetComponent<ParticleSystem>();
            elementRenderer = gameObject.GetComponent<Renderer>();

            if (customMaterial != null)
            {
                elementMaterial = customMaterial;
            }
            else
            {
                elementMaterial = elementRenderer.sharedMaterial;
            }

            if (elementMaterial == null || elementMaterial.name == "Element")
            {
                if (materialData == null)
                {
                    materialData = new TVEElementMaterialData();
                }

                if (materialData.shader == null)
                {
#if UNITY_EDITOR
                    elementMaterial = new Material(Resources.Load<Material>("Internal Colors"));
                    SaveMaterialData(elementMaterial);
#endif
                }
                else
                {
                    elementMaterial = new Material(materialData.shader);
                    LoadMaterialData(elementMaterial);
                }

                elementMaterial.name = "Element";
                gameObject.GetComponent<Renderer>().sharedMaterial = elementMaterial;
            }
        }

        void OnDestroy()
        {
            isActive = false;
        }

        void OnDisable()
        {
            isActive = false;

#if UNITY_EDITOR
            debugData = "<size=10>The element is disabled</size>";
#endif
        }

        void Update()
        {
            if (TVEManager.Instance == null)
            {
                isActive = false;

#if UNITY_EDITOR
                debugData = "<size=10>The element is disabled because the manager is missing</size>";
#endif
                return;
            }

            if (!isActive)
            {
                UpdateElement();

                isActive = true;
            }

#if UNITY_EDITOR
            if (Selection.Contains(gameObject))
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }

            if (isSelected)
            {
                UpdateElement();
            }

            // Requires updating when the terrain is changed
            if (!Application.isPlaying)
            {
                if (terrainData != null && terrainMask != 0)
                {
                    UpdateElement();
                }
            }
#endif

            UpdateFading();
        }

        void UpdateElement()
        {
            elementID = this.GetHashCode();

            if (customMaterial != null)
            {
                elementMaterial = customMaterial;
            }
            else
            {
                elementMaterial = elementRenderer.sharedMaterial;
            }

            if (elementMaterial != null)
            {
                TVEUtils.SetElementSettings(elementMaterial);
                TVEUtils.CopyTerrainDataToElement(terrainData, terrainMask, elementMaterial);

                renderName = elementMaterial.GetTag("ElementType", false);
                renderDataID = renderName.GetHashCode();

                GetMaterialParameters();

#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(elementMaterial))
                {
                    SaveMaterialData(elementMaterial);
                }
#endif
            }

            var meshFilter = gameObject.GetComponent<MeshFilter>();

            if (meshFilter != null)
            {
                elementMesh = meshFilter.sharedMesh;
            }

            if (elementMesh != null && elementMaterial.enableInstancing == true)
            {
                instancedID = elementMesh.GetHashCode() + elementMaterial.GetHashCode();
            }

            AddElementToVolume();
            SetElementVisibility();

#if UNITY_EDITOR
            TVEManager.Instance.MarkSortDirty(); // SortElementObjects();

            UpdateDebugData();
#endif
        }

        void UpdateFading()
        {
            if (particleSystem != null)
            {
                var particleModule = particleSystem.main;
                var particleColor = particleModule.startColor.color;

                if (useVertexColorDirection > 0)
                {
                    var direction = transform.position - lastPosition;
                    var localDirection = transform.InverseTransformDirection(direction);
                    var worldDirection = transform.TransformVector(localDirection);
                    lastPosition = transform.position;

                    var worldDirectionX = Mathf.Clamp01(worldDirection.x * speedTreshold * 0.5f + 0.5f);
                    var worldDirectionZ = Mathf.Clamp01(worldDirection.z * speedTreshold * 0.5f + 0.5f);

                    particleColor = new Color(worldDirectionX, worldDirectionZ, particleColor.b, particleColor.a);
                }

                if (useRaycastFading > 0)
                {
                    var fade = GetRacastFading();
                    particleColor = new Color(particleColor.r, particleColor.g, particleColor.b, particleColor.a * fade);
                }
                else
                {
                    particleColor = new Color(particleColor.r, particleColor.g, particleColor.b, particleColor.a);
                }

                particleModule.startColor = particleColor;
            }
            else
            {
                if (useRaycastFading > 0)
                {
                    var fade = GetRacastFading();
                    elementParams.w = fade;
                }
                else
                {
                    elementParams.w = 1;
                }
            }
        }

        void AddElementToVolume()
        {
            var renderDataSet = TVEManager.Instance.renderDataSet;
            var elementObjects = TVEManager.Instance.renderElements;
            var elementInstances = TVEManager.Instance.renderInstances;
            
            var renderDataExists = false;            

            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData.renderDataID == renderDataID)
                {
                    renderDataExists = true;
                    break;
                }
            }

            if (!renderDataExists)
            {
                TVEManager.Instance.CreateRenderData(renderName);
            }

            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null)
                {
                    continue;
                }

                if (renderData.renderDataID != renderDataID)
                {
                    continue;
                }

                var maxLayer = 0;
                renderLayers = new List<int>(9);

                if (renderData.textureArray == TVEBool.On && elementMaterial.HasProperty("_ElementLayerMask"))
                {
                    var bitmask = elementMaterial.GetInt("_ElementLayerMask");

                    for (int m = 0; m < 9; m++)
                    {
                        if (((1 << m) & bitmask) != 0)
                        {
                            renderLayers.Add(1);
                            maxLayer = m;
                        }
                        else
                        {
                            renderLayers.Add(0);
                        }
                    }
                }
                else
                {
                    renderLayers.Add(1);

                    for (int m = 1; m < 9; m++)
                    {
                        renderLayers.Add(0);
                    }
                }

                if (maxLayer > renderData.bufferSize)
                {
                    renderData.bufferSize = maxLayer;
                    TVEManager.Instance.CreateRenderBuffer(renderData);
                }

                if (Application.isPlaying && SystemInfo.supportsInstancing)
                {
                    if (instancedID == 0)
                    {
                        bool containsElement = false;

                        for (int e = 0; e < elementObjects.Count; e++)
                        {
                            if (elementObjects[e].elementID == elementID)
                            {
                                containsElement = true;
                                break;
                            }
                        }

                        if (!containsElement)
                        {
                            elementObjects.Add(this);
                        }
                    }
                    else
                    {
                        bool containsElement = false;
                        int index = 0;

                        for (int e = 0; e < elementInstances.Count; e++)
                        {
                            if (elementInstances[e].renderers.Count > 1022)
                            {
                                continue;
                            }

                            if (elementInstances[e].instancedDataID == instancedID)
                            {
                                containsElement = true;
                                index = e;
                                break;

                            }
                        }

                        if (!containsElement)
                        {
                            var elementInstanced = new TVEInstanced();
                            elementInstanced.instancedDataID = instancedID;
                            elementInstanced.renderDataID = renderDataID;
                            elementInstanced.renderLayers = renderLayers;
                            elementInstanced.renderPass = renderPass;
                            elementInstanced.material = elementMaterial;
                            elementInstanced.mesh = elementMesh;
                            elementInstanced.elements.Add(this);
                            elementInstanced.renderers.Add(elementRenderer);

                            elementInstances.Add(elementInstanced);
                        }
                        else
                        {
                            bool containsRenderer = false;

                            for (int r = 0; r < elementInstances[index].renderers.Count; r++)
                            {
                                if (elementInstances[index].renderers[r] == elementRenderer)
                                {
                                    containsRenderer = true;
                                    break;
                                }
                            }

                            if (!containsRenderer)
                            {
                                elementInstances[index].elements.Add(this);
                                elementInstances[index].renderers.Add(elementRenderer);
                            }
                        }
                    }
                }
                else
                {
                    bool containsElement = false;

                    for (int e = 0; e < elementObjects.Count; e++)
                    {
                        if (elementObjects[e].elementID == elementID)
                        {
                            containsElement = true;
                            break;
                        }
                    }

                    if (!containsElement)
                    {
                        elementObjects.Add(this);
                    }
                }
            }
        }

        void SetElementVisibility()
        {
            if (customVisibility == TVEElementVisibility.UseGlobalSettings)
            {
                var visibility = TVEManager.Instance.elementVisibility;

                if (visibility == TVEElementsVisibility.AlwaysHidden)
                {
                    elementRenderer.enabled = false;
                }

                if (visibility == TVEElementsVisibility.AlwaysVisible)
                {
                    elementRenderer.enabled = true;
                }

                if (visibility == TVEElementsVisibility.HiddenAtRuntime)
                {
                    if (Application.isPlaying)
                    {
                        elementRenderer.enabled = false;
                    }
                    else
                    {
                        elementRenderer.enabled = true;
                    }
                }
            }
            else
            {
                if (customVisibility == TVEElementVisibility.AlwaysHidden)
                {
                    elementRenderer.enabled = false;
                }

                if (customVisibility == TVEElementVisibility.AlwaysVisible)
                {
                    elementRenderer.enabled = true;
                }

                if (customVisibility == TVEElementVisibility.HiddenAtRuntime)
                {
                    if (Application.isPlaying)
                    {
                        elementRenderer.enabled = false;
                    }
                    else
                    {
                        elementRenderer.enabled = true;
                    }
                }
            }
        }

#if UNITY_EDITOR
        void UpdateDebugData()
        {
            var tveManager = TVEManager.Instance;

            if (tveManager == null)
            {
                return;
            }

            var volumeScale = new Vector3(tveManager.renderBaseDistance * 2, tveManager.renderBaseDistance * 2, tveManager.renderBaseDistance * 2);
            var volumeBounds = new Bounds(tveManager.focusPosition, volumeScale);
            var elementBounds = elementRenderer.bounds;

            if (volumeBounds.Intersects(elementBounds))
            {
                if (volumeBounds.Contains(elementBounds.min) && volumeBounds.Contains(elementBounds.max))
                {
                    if (EditorGUIUtility.isProSkin)
                    {
                        debugData = "<size=10><color=#e1f043>The element is inside the base distance bounds</color></size>";
                    }
                    else
                    {
                        debugData = "<size=10>The element is inside the base distance bounds</size>";
                    }
                }
                else
                {
                    if (EditorGUIUtility.isProSkin)
                    {
                        debugData = "<size=10><color=#ffdb78>The element is only partially inside the base distance bounds</color></size>";
                    }
                    else
                    {
                        debugData = "<size=10>The element is only partially inside the base distance bounds</size>";
                    }
                }
            }
            else
            {
                if (EditorGUIUtility.isProSkin)
                {
                    debugData = "<b><size=10><color=#ff7663>The element is outside the base distance bounds</color></size></b>";
                }
                else
                {
                    debugData = "<b><size=10><color=#e82c02>The element is outside the base distance bounds</color></size></b>";
                }
            }
        }

        void SaveMaterialData(Material material)
        {
            materialData = new TVEElementMaterialData();
            materialData.props = new List<TVEElementPropertyData>();

            materialData.shader = material.shader;
            materialData.shaderName = material.shader.name;

            for (int i = 0; i < ShaderUtil.GetPropertyCount(material.shader); i++)
            {
                var type = ShaderUtil.GetPropertyType(material.shader, i);
                var prop = ShaderUtil.GetPropertyName(material.shader, i);

                if (type == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = TVEPropertyType.Texture;
                    propData.prop = prop;
                    propData.texture = material.GetTexture(prop);

                    materialData.props.Add(propData);
                }

                if (type == ShaderUtil.ShaderPropertyType.Vector || type == ShaderUtil.ShaderPropertyType.Color)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = TVEPropertyType.Vector;
                    propData.prop = prop;
                    propData.vector = material.GetVector(prop);

                    materialData.props.Add(propData);
                }

                if (type == ShaderUtil.ShaderPropertyType.Float || type == ShaderUtil.ShaderPropertyType.Range)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = TVEPropertyType.Value;
                    propData.prop = prop;
                    propData.value = material.GetFloat(prop);

                    materialData.props.Add(propData);
                }
            }
        }
#endif

        void LoadMaterialData(Material material)
        {
            material.shader = materialData.shader;

            for (int i = 0; i < materialData.props.Count; i++)
            {
                if (materialData.props[i].type == TVEPropertyType.Texture)
                {
                    material.SetTexture(materialData.props[i].prop, materialData.props[i].texture);
                }

                if (materialData.props[i].type == TVEPropertyType.Vector)
                {
                    material.SetVector(materialData.props[i].prop, materialData.props[i].vector);
                }

                if (materialData.props[i].type == TVEPropertyType.Value)
                {
                    material.SetFloat(materialData.props[i].prop, materialData.props[i].value);
                }
            }
        }

        void GetMaterialParameters()
        {
            if (elementMaterial.HasProperty("_ElementPassValue"))
            {
                renderPass = elementMaterial.GetInt("_ElementPassValue");
            }

            if (elementMaterial.HasProperty("_MotionDirectionMode"))
            {
                if (elementMaterial.GetInt("_MotionDirectionMode") == 2)
                {
                    useVertexColorDirection = 1;
                }
                else
                {
                    useVertexColorDirection = 0;
                }
            }

            if (elementMaterial.HasProperty("_SpeedTresholdValue"))
            {
                speedTreshold = elementMaterial.GetFloat("_SpeedTresholdValue");
            }

            if (elementMaterial.HasProperty("_ElementRaycastMode"))
            {
                useRaycastFading = elementMaterial.GetInt("_ElementRaycastMode");
                raycastMask = elementMaterial.GetInt("_RaycastLayerMask");
                raycastEnd = elementMaterial.GetFloat("_RaycastDistanceMaxValue");
            }
        }

        float GetRacastFading()
        {
            raycastEnd = elementMaterial.GetFloat("_RaycastDistanceMaxValue");

            RaycastHit hit;
            bool raycastHit = Physics.Raycast(transform.position, -Vector3.up, out hit, raycastEnd, raycastMask);

            if (hit.distance > 0)
            {
                return 1 - Mathf.Clamp01(hit.distance / raycastEnd);
            }
            else
            {
                return 0;
            }
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }

        void OnDrawGizmos()
        {
            DrawGizmos(false);
        }

        void DrawGizmos(bool selected)
        {
            if (TVEManager.Instance == null || !isActive)
            {
                return;
            }

            var genericColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
            //var invalidColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

            if (selected)
            {
                genericColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                //invalidColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }

            Gizmos.color = genericColor;

            if (isSelected)
            {
                if (useRaycastFading > 0)
                {
                    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - raycastEnd, transform.position.z));
                }
            }

            Bounds gizmoBounds;

            if (elementMesh != null)
            {
                gizmoBounds = elementMesh.bounds;
                Gizmos.matrix = transform.localToWorldMatrix;
            }
            else
            {
                gizmoBounds = elementRenderer.bounds;
            }

            Gizmos.DrawWireCube(gizmoBounds.center, gizmoBounds.size);
        }
    }
}
