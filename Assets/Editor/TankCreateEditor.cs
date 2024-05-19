using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TankCreateEditor : EditorWindow
{
    private GameObject _vehicle;
    private Transform _newVenicle;

    private Mesh _roadWheelMesh;
    private Material[] _roadWheelMaterials;

    private Mesh _suspensionLMesh;
    private Mesh _suspensionRMesh;
    public Material[] _suspensionMaterials;

    private Mesh _IdlerWheelMesh;
    private Material[] _IdlerWheelMaterials;

    private Mesh _spoketWheelMesh;
    public Material[] _spoketWheelMaterial;

    private Mesh _trackBeltMesh;
    public Material[] _trackBeltMaterial;

    private Mesh _barrelMesh;
    private Material[] _barrelMaterials;

    private Mesh _turretMesh;
    private Material[] _turretMaterials;

    private Mesh _cannonMesh;
    private Material[] _cannonMaterials;

    private Mesh _mainBodyMesh;
    public Material[] _mainBodyMaterials;

    private Vector3 _wheelsInitialAngles;

    [MenuItem("TankBeta/TankCreateEditor")]
    public static void ShowWindow()
    {
        GetWindow<TankCreateEditor>();

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Create"))
        {
            Create();
        }
    }

    private void Create()
    {
        Find();
        if (_vehicle == null)
        {
            Debug.LogError("cant find veicle");
            return; 
        }

        var turretOffset = GetOffset(ObjectType.turret);
        var cannonOffset = GetOffset(ObjectType.cannon);
        var barrelOffset = GetOffset(ObjectType.barrel);

        var crw = _vehicle.GetComponentInChildren<Create_RoadWheel_CS>();
        crw.wheelsInitialAngles = _wheelsInitialAngles;
        crw.Wheel_Mesh = _roadWheelMesh;
        crw.Sus_L_Mesh = _suspensionLMesh;
        crw.Sus_R_Mesh = _suspensionRMesh;
        crw.Sus_Materials = _suspensionMaterials;
        crw.Wheel_Materials = _roadWheelMaterials;
        crw.Wheel_Material = _roadWheelMaterials[0];
        crw.Wheel_Materials_Num = _roadWheelMaterials.Length;
        crw.Create();

        var ciw = _vehicle.GetComponentInChildren<Create_IdlerWheel_CS>();
        ciw.Wheel_Mesh = _IdlerWheelMesh;
        ciw.Wheel_Materials[0] = _IdlerWheelMaterials[0];
        ciw.Wheel_Materials_Num = _IdlerWheelMaterials.Length;
        ciw.Wheel_Material = _IdlerWheelMaterials[0];
        ciw.Create();

        var csw = _vehicle.GetComponentInChildren<Create_SprocketWheel_CS>();
        csw.Wheel_Mesh = _spoketWheelMesh;
        csw.Wheel_Material = _spoketWheelMaterial[0];
        csw.Wheel_Materials_Num = _spoketWheelMaterial.Length;
        csw.Wheel_Materials = _spoketWheelMaterial;
        csw.Create();

        var tracs = _vehicle.GetComponentInChildren<Static_Track_Parent_CS>().GetComponentsInChildren<Static_Track_Piece_CS>();
        if (tracs != null)
        {
            foreach (var track in tracs)
            {
                track.gameObject.GetComponent<MeshFilter>().mesh = _trackBeltMesh;
                track.gameObject.GetComponent<MeshRenderer>().materials = _trackBeltMaterial;
            }
        }
        var bb = _vehicle.GetComponentInChildren<Barrel_Base_CS>();
        bb.Part_Mesh = _barrelMesh;
        bb.Part_Material = _barrelMaterials[0];
        bb.Materials = _barrelMaterials;
        bb.Offset_X = barrelOffset.x;
        bb.Offset_Y = barrelOffset.y;
        bb.Offset_Z = barrelOffset.z;
        bb.Create();

        var ct = _vehicle.GetComponentInChildren<Turret_Base_CS>();
        ct.Part_Mesh = _turretMesh;
        ct.Materials = _turretMaterials;
        ct.Materials_Num = 1;
        ct.Offset_X = turretOffset.x;
        ct.Offset_Y = turretOffset.y;
        ct.Offset_Z = turretOffset.z;
        ct.Create();

        var cb = _vehicle.GetComponentInChildren<Cannon_Base_CS>();
        cb.Part_Mesh = _cannonMesh;
        cb.Materials = _cannonMaterials;
        cb.Part_Material = _cannonMaterials[0];
        cb.Offset_X = cannonOffset.x;
        cb.Offset_Y = cannonOffset.y;
        cb.Offset_Z = cannonOffset.z;
        cb.Create();

        var mbs = _vehicle.GetComponentInChildren<MainBody_Setting_CS>();
        mbs.Body_Mesh = _mainBodyMesh;
        mbs.Materials = _mainBodyMaterials;
        mbs.Materials_Num = _mainBodyMaterials.Length;
        mbs.Create();


        Vector3 GetOffset(ObjectType type)
        {
            var objs = FindObjectsOfType<ObjectToFind>();          
            foreach (var obj in objs)
            {
                var resours = obj.GetResources();
                if (resours.type == type)
                {
                    return obj.transform.localPosition;
                }              
            }
            return Vector3.zero;
        }
        void Find()
        {
            var objs = FindObjectsOfType<ObjectToFind>();
            foreach (var obj in objs) 
            {
                switch (obj.GetType())
                {
                    case ObjectType.body:  
                        
                        _mainBodyMesh = obj.GetResources()._mesh;
                        _mainBodyMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.turret:                       
                        _turretMesh = obj.GetResources()._mesh;
                        _turretMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.cannon:
                        _cannonMesh = obj.GetResources()._mesh;
                        _cannonMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.barrel:
                        _barrelMesh = obj.GetResources()._mesh;
                        _barrelMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.track:
                        _trackBeltMesh = obj.GetResources()._mesh;
                        _trackBeltMaterial = obj.GetResources()._materials;
                        break;
                    case ObjectType.spoketWheel:
                        _spoketWheelMesh = obj.GetResources()._mesh;
                        _spoketWheelMaterial = obj.GetResources()._materials;
                        break;
                    case ObjectType.idlerWheel:
                        _IdlerWheelMesh = obj.GetResources()._mesh;
                        _IdlerWheelMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.suspentionR:
                        _suspensionRMesh = obj.GetResources()._mesh;
                        _suspensionMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.suspentionL:
                        _suspensionLMesh = obj.GetResources()._mesh;
                        _suspensionMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.Wheel:
                        _wheelsInitialAngles = obj.transform.localEulerAngles;
                        _roadWheelMesh = obj.GetResources()._mesh;
                        _roadWheelMaterials = obj.GetResources()._materials;
                        break;
                    case ObjectType.Tank:
                        _vehicle = obj.gameObject;
                        break;
                    case ObjectType.ERA:
                        obj.gameObject.AddComponent<ExplosiveReactiveArmour>();
                        obj.gameObject.AddComponent<MeshCollider>();
                        break;
                    case ObjectType.newTank:
                        _newVenicle = obj.transform;
                        break;
                }
                
            }
            for (int i = 0; i < _newVenicle.GetChild(0).childCount; i++)
            {
                var obj = _newVenicle.transform.GetChild(0).GetChild(i).gameObject;
                if (obj.GetComponent<ObjectToFind>() != null) continue;
                var element = Instantiate(obj, _vehicle.transform.GetChild(0).transform).transform;
                element.localPosition = obj.transform.localPosition;
                element.localEulerAngles = obj.transform.localEulerAngles;
            }
        }
    }
}