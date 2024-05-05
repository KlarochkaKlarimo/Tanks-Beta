using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TankCreateEditor : EditorWindow
{
    private GameObject _vehicle;
    private GameObject _meshew;

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
        Find(ref _vehicle,"Tank");
        var turretOffset = GetOffset(ObjectType.turret);
        var cannonOffset = GetOffset(ObjectType.cannon);
        var barrelOffset = GetOffset(ObjectType.barrel);

        //SetElements(new List<string>() { "Wheel", "IdlerWheel", "SpoketWheel", "Track", "Barrel", "Turret", "Cannon" }, new List<string>() { "ERA" });
        //_mainBodyMesh = _meshew.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        //_mainBodyMaterials = _meshew.transform.GetChild(0).GetComponent<MeshRenderer>().materials;

        var crw = _vehicle.GetComponentInChildren<Create_RoadWheel_CS>();
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


        //   var ctb = _vehicle.GetComponentInChildren<Create_TrackBelt_CS>();
        var tracs = _vehicle.GetComponentInChildren<Static_Track_Parent_CS>().GetComponentsInChildren<Static_Track_Piece_CS>();
        foreach (var track in tracs)
        {
            var mesh = track.gameObject.GetComponent<MeshFilter>().mesh = _trackBeltMesh;
            track.gameObject.GetComponent<MeshRenderer>().materials = _trackBeltMaterial;
        }
        //if (ctb != null)
        //{
        //    ctb.Track_L_Mesh = _trackBeltMesh;
        //    ctb.Track_Materials = _trackBeltMaterial;
        //    ctb.Track_Materials_Num = _trackBeltMaterial.Length;
        //    ctb.Create();
        //}
       
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
            /*_meshew.transform.GetChild(0).Find(name).transform.localPosition; */
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
        void FindTankElement(ref Mesh mesh, ref Material[] materials, string elementName)
        {
            var parent = _meshew.transform.GetChild(0);
            var element = _meshew.transform.GetChild(0).Find(elementName);
            if (element != null)
            {
                mesh = element.GetComponent<MeshFilter>().mesh;
                materials = element.GetComponent<MeshRenderer>().materials;
                return;
            }
            Debug.Log("Ther is no " + elementName);
        }
        void Find(ref GameObject _object, string objName)
        {
            var objs = FindObjectsOfType<ObjectToFind>();
            Debug.Log("lenght " + objs.Length);
            foreach (var obj in objs) 
            {
                var resours = obj.GetResources();
                switch (obj.GetResources().type)
                {
                    case ObjectType.body:                      
                        _mainBodyMesh = resours._mesh;
                        _mainBodyMaterials = resours._materials;
                        break;

                    case ObjectType.turret:                       
                        _turretMesh = resours._mesh;
                        _turretMaterials = resours._materials;
                        break;

                    case ObjectType.cannon:
                        _cannonMesh = resours._mesh;
                        _cannonMaterials = resours._materials;
                        break;

                    case ObjectType.barrel:
                        _barrelMesh = resours._mesh;
                        _barrelMaterials = resours._materials;
                        break;

                    case ObjectType.track:
                        _trackBeltMesh = resours._mesh;
                        _trackBeltMaterial = resours._materials;
                        break;

                    case ObjectType.spoketWheel:
                        _spoketWheelMesh = resours._mesh;
                        _spoketWheelMaterial = resours._materials;
                        break;

                    case ObjectType.idlerWheel:
                        _IdlerWheelMesh = resours._mesh;
                        _IdlerWheelMaterials = resours._materials;
                        break;

                    case ObjectType.suspentionR:
                        _suspensionRMesh = resours._mesh;
                        _suspensionMaterials = resours._materials;
                        break;

                    case ObjectType.suspentionL:
                        _suspensionLMesh = resours._mesh;
                        _suspensionMaterials = resours._materials;
                        break;

                    case ObjectType.Wheel:
                        _roadWheelMesh = resours._mesh;
                        _roadWheelMaterials = resours._materials;
                        break;
                }
                
            }
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
                            _object = obj;
                            return;
                        }
                    }
                }
            }
            Debug.Log("Cant find obj");
        }
        void SetElements(List<string> list , List<string> elementsNames)
        {
            for (int i = 0; i < _meshew.transform.GetChild(0).childCount; i++)
            {
                var obj = _meshew.transform.GetChild(0).GetChild(i).gameObject;    
                if (!list.Contains(obj.name)&& obj.activeInHierarchy)
                {
                  var element = Instantiate(obj, _vehicle.transform.GetChild(0).transform);
                  element.transform.localPosition = obj.transform.localPosition;
                  element.transform.localEulerAngles = obj.transform.localEulerAngles;
                  var words = obj.name.Split(' ');

                    foreach (var word in words)
                    {
                        if (elementsNames.Contains(word))
                        {
                            switch (word)
                            {
                                case "ERA":
                                    element.gameObject.AddComponent<ExplosiveReactiveArmour>();
                                    element.gameObject.AddComponent<MeshCollider>();
                                    break;
                            }
                        }
                    }
                 }
            }

        }
    }


}
