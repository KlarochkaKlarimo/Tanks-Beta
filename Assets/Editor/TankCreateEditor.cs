using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.SceneManagement;

public class TankCreateEditor : EditorWindow
{
    private GameObject _vehicle;

    private Mesh _roadWheelsMesh;
    private Material _roadWheelsMaterial;

    private Mesh _IlderWheelMesh;
    private Material _IlderWheelMaterial;

    private Mesh _sproketWheelMesh;
    private Material _sproketWheelMaterial;

    private Mesh _trackBeltMesh;
    private Material _trackBeltMaterial;

    private Mesh _barrelMesh;
    private Material _barrelMaterial;

    private Mesh _turretMesh;
    private Material _turretMaterial;

    private Mesh _cannonMesh;
    private Material _cannonMaterial;

    private Mesh _mainBodyMesh;
    private Material _mainBodyMaterial;

    //private Mesh
    //private Material

    [MenuItem("TankBeta/TankCreateEditor")]
    public static void ShowWindow()
    {
        GetWindow<TankCreateEditor>();


    }

    private void OnGUI()
    {

        _roadWheelsMesh = (Mesh)EditorGUILayout.ObjectField("каток", _roadWheelsMesh, typeof(Mesh), false);
        _roadWheelsMaterial = (Material)EditorGUILayout.ObjectField("каток", _roadWheelsMaterial, typeof(Material), false);

        _IlderWheelMesh = (Mesh)EditorGUILayout.ObjectField("ведущий каток", _IlderWheelMesh, typeof(Mesh), false);
        _IlderWheelMaterial = (Material)EditorGUILayout.ObjectField("ведущий каток", _IlderWheelMaterial, typeof(Material), false);

        _sproketWheelMesh = (Mesh)EditorGUILayout.ObjectField("силовой каток", _sproketWheelMesh, typeof(Mesh), false);
        _sproketWheelMaterial = (Material)EditorGUILayout.ObjectField("силовой каток", _sproketWheelMaterial, typeof(Material), false);

        _trackBeltMesh = (Mesh)EditorGUILayout.ObjectField("гусля", _trackBeltMesh, typeof(Mesh), false);
        _trackBeltMaterial = (Material)EditorGUILayout.ObjectField("гусля", _trackBeltMaterial, typeof(Material), false);

        _barrelMesh = (Mesh)EditorGUILayout.ObjectField("ствол пушки", _barrelMesh, typeof(Mesh), false);
        _barrelMaterial = (Material)EditorGUILayout.ObjectField("ствол пушки", _barrelMaterial, typeof(Material), false);

        _turretMesh = (Mesh)EditorGUILayout.ObjectField("башня", _turretMesh, typeof(Mesh), false);
        _turretMaterial = (Material)EditorGUILayout.ObjectField("башня", _turretMaterial, typeof(Material), false);

        _cannonMesh = (Mesh)EditorGUILayout.ObjectField("маска орудия", _cannonMesh, typeof(Mesh), false);
        _cannonMaterial = (Material)EditorGUILayout.ObjectField("маска орудия", _cannonMaterial, typeof(Material), false);

        _mainBodyMesh = (Mesh)EditorGUILayout.ObjectField("корпус", _mainBodyMesh, typeof(Mesh), false);
        _mainBodyMaterial = (Material)EditorGUILayout.ObjectField("корпус", _mainBodyMaterial, typeof(Material), false);

        if (GUILayout.Button("Create"))
        {
            Create();
        }
    }

    private void Create()
    {
        Find("Tank");
        var crw = _vehicle.GetComponentInChildren<Create_RoadWheel_CS>();
        crw.Wheel_Mesh = _roadWheelsMesh;
        crw.Wheel_Material = _roadWheelsMaterial;
        crw.Create();

        var ciw = _vehicle.GetComponentInChildren<Create_IdlerWheel_CS>();
        ciw.Wheel_Mesh = _IlderWheelMesh;
        ciw.Wheel_Material = _IlderWheelMaterial;

        var csw = _vehicle.GetComponentInChildren<Create_SprocketWheel_CS>();
        csw.Wheel_Mesh = _sproketWheelMesh;
        csw.Wheel_Material = _sproketWheelMaterial;

        var ctb = _vehicle.GetComponentInChildren<Create_TrackBelt_CS>();
        ctb.Track_L_Mesh = _trackBeltMesh;
        ctb.Track_Materials[0] = _sproketWheelMaterial;

        var bb = _vehicle.GetComponentInChildren<Barrel_Base_CS>();
        bb.Part_Mesh = _barrelMesh;
        bb.Part_Material = _barrelMaterial;

        var ct = _vehicle.GetComponentInChildren<Turret_Base_CS>();
        ct.Part_Mesh = _turretMesh;
        ct.Part_Material = _turretMaterial;

        var cb = _vehicle.GetComponentInChildren<Cannon_Base_CS>();
        cb.Part_Mesh = _cannonMesh;
        cb.Part_Material = _cannonMaterial;

        //var mbs = _vehicle.GetComponentInChildren<MainBody_Setting_CSEditor>();
        //mbs.Body_MeshProp.objectReferenceValue = _mainBodyMesh;
        //  mbs.MaterialsProp.objectReferenceValue = _mainBodyMaterial;
        var mbs = _vehicle.GetComponentInChildren<MainBody_Setting_CS>();
        mbs.Body_Mesh = _mainBodyMesh;
        mbs.Materials[0] = _mainBodyMaterial;
        mbs.Create();
    }
    private void Find(string objName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                foreach (var obj in allGameObjects)
                {
                    if (obj.name == objName)
                    {
                        _vehicle = obj;
                        return;
                    }
                }
            }
        }
        Debug.Log("Cant find obj");
    }

}
