using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class TankCreateEditor : EditorWindow
{
    private GameObject _vehicle;
    private GameObject _meshew;

    private Mesh _roadWheelsMesh;
    private Material[] _roadWheelsMaterials;

    private Mesh _IlderWheelMesh;
    private Material[] _IlderWheelMaterials;

    private Mesh _sproketWheelMesh;
    public Material[] _sproketWheelMaterial;

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
        Find(ref _meshew, "TestTank");

        FindTankElement(ref _roadWheelsMesh,ref _roadWheelsMaterials, "Wheel");
        FindTankElement(ref _IlderWheelMesh, ref _IlderWheelMaterials, "IdlerWheel");
        FindTankElement(ref _sproketWheelMesh, ref _sproketWheelMaterial, "SpoketWheel");
        FindTankElement(ref _trackBeltMesh, ref _trackBeltMaterial, "Track");

        FindTankElement(ref _barrelMesh, ref _barrelMaterials, "Barrel");
        FindTankElement(ref _turretMesh, ref _turretMaterials, "Turret");
        FindTankElement(ref _cannonMesh, ref _cannonMaterials, "Cannon");

        var turretOffset = GetOffset("Turret");
        var cannonOffset = GetOffset("Cannon");
        var barrelOffset = GetOffset("Barrel");

        SetElements(new List<string>() { "Wheel", "IdlerWheel", "SpoketWheel" , "Track" , "Barrel" , "Turret", "Cannon" },new List<string>() { "ERA" });
        _mainBodyMesh = _meshew.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        _mainBodyMaterials = _meshew.transform.GetChild(0).GetComponent<MeshRenderer>().materials;

        var crw = _vehicle.GetComponentInChildren<Create_RoadWheel_CS>();
        crw.Wheel_Mesh = _roadWheelsMesh;
        crw.Wheel_Materials = _roadWheelsMaterials;
        crw.Wheel_Material = _roadWheelsMaterials[0];
        crw.Wheel_Materials_Num = _roadWheelsMaterials.Length;
        crw.Create();

        var ciw = _vehicle.GetComponentInChildren<Create_IdlerWheel_CS>();
        ciw.Wheel_Mesh = _IlderWheelMesh;
        ciw.Wheel_Materials[0] = _IlderWheelMaterials[0];
        ciw.Wheel_Materials_Num = _IlderWheelMaterials.Length;
        ciw.Wheel_Material = _IlderWheelMaterials[0];
        ciw.Create();

        var csw = _vehicle.GetComponentInChildren<Create_SprocketWheel_CS>();
        csw.Wheel_Mesh = _sproketWheelMesh;
        csw.Wheel_Material = _sproketWheelMaterial[0];
        csw.Wheel_Materials_Num = _sproketWheelMaterial.Length;
        csw.Wheel_Materials = _sproketWheelMaterial;
        csw.Create();

        var ctb = _vehicle.GetComponentInChildren<Create_TrackBelt_CS>();
        ctb.Track_L_Mesh = _trackBeltMesh;
        ctb.Track_Materials = _trackBeltMaterial;
        ctb.Track_Materials_Num = _trackBeltMaterial.Length;
        ctb.Create();

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


        Vector3 GetOffset(string name) { return _meshew.transform.GetChild(0).Find(name).transform.localPosition; }
        void FindTankElement(ref Mesh mesh, ref Material[] materials, string elementName)
        {
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
