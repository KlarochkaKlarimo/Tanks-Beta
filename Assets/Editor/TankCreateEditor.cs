using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.SceneManagement;
using UnityEditor.Tanks;

public class TankCreateEditor : EditorWindow
{
    private GameObject _vehicle;

    private Mesh _roadWheelsMesh;
    private Material _roadWheelsMaterial;

    private Mesh _IlderWheelMesh;
    private Material _IlderWheelMaterial;

    private Mesh _sproketWheelMesh;
    public Material[] _sproketWheelMaterial;

    private Mesh _trackBeltMesh;
    public Material[] _trackBeltMaterial;

    private Mesh _barrelMesh;
    private Material _barrelMaterial;

    private Mesh _turretMesh;
    private Material _turretMaterial;
    private Vector3 _turretOffset;

    private Mesh _cannonMesh;
    private Material _cannonMaterial;

    private Mesh _mainBodyMesh;
    public Material[] _mainBodyMaterial;

    private SerializedObject serializedObject;
    //private Mesh
    //private Material

    [MenuItem("TankBeta/TankCreateEditor")]
    public static void ShowWindow()
    {
        GetWindow<TankCreateEditor>();

    }
    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
    }
    private void OnGUI()
    {
        _roadWheelsMesh = (Mesh)EditorGUILayout.ObjectField("каток", _roadWheelsMesh, typeof(Mesh), false);
        _roadWheelsMaterial = (Material)EditorGUILayout.ObjectField("каток", _roadWheelsMaterial, typeof(Material), false);

        _IlderWheelMesh = (Mesh)EditorGUILayout.ObjectField("ведущий каток", _IlderWheelMesh, typeof(Mesh), false);
        _IlderWheelMaterial = (Material)EditorGUILayout.ObjectField("ведущий каток", _IlderWheelMaterial, typeof(Material), false);

        _sproketWheelMesh = (Mesh)EditorGUILayout.ObjectField("силовой каток", _sproketWheelMesh, typeof(Mesh), false);
        // _sproketWheelMaterial = (Material)EditorGUILayout.ObjectField("силовой каток", _sproketWheelMaterial, typeof(Material), false);
        serializedObject.GenerateEditorArray("_sproketWheelMaterial");

        _trackBeltMesh = (Mesh)EditorGUILayout.ObjectField("гусля", _trackBeltMesh, typeof(Mesh), false);
        serializedObject.GenerateEditorArray("_trackBeltMaterial"); 
        //_trackBeltMaterial = (Material)EditorGUILayout.ObjectField("гусля", _trackBeltMaterial, typeof(Material), false);

        _barrelMesh = (Mesh)EditorGUILayout.ObjectField("ствол пушки", _barrelMesh, typeof(Mesh), false);
        _barrelMaterial = (Material)EditorGUILayout.ObjectField("ствол пушки", _barrelMaterial, typeof(Material), false);

        _turretMesh = (Mesh)EditorGUILayout.ObjectField("башня", _turretMesh, typeof(Mesh), false);
        _turretMaterial = (Material)EditorGUILayout.ObjectField("башня", _turretMaterial, typeof(Material), false);
        _turretOffset = (Vector3)EditorGUILayout.Vector3Field("позиция башни", _turretOffset);
        _cannonMesh = (Mesh)EditorGUILayout.ObjectField("маска орудия", _cannonMesh, typeof(Mesh), false);
        _cannonMaterial = (Material)EditorGUILayout.ObjectField("маска орудия", _cannonMaterial, typeof(Material), false);

        _mainBodyMesh = (Mesh)EditorGUILayout.ObjectField("корпус", _mainBodyMesh, typeof(Mesh), false);
        serializedObject.GenerateEditorArray("_mainBodyMaterial");
        //_mainBodyMaterial = (Material)EditorGUILayout.ObjectField("корпус", _mainBodyMaterial, typeof(Material), false);

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
        crw.Wheel_Materials[0] = _roadWheelsMaterial;
        crw.Wheel_Material = _roadWheelsMaterial;
        crw.Wheel_Materials_Num = 1;
        crw.Create();

        var ciw = _vehicle.GetComponentInChildren<Create_IdlerWheel_CS>();
        ciw.Wheel_Mesh = _IlderWheelMesh;
        ciw.Wheel_Materials[0] = _IlderWheelMaterial;
        ciw.Wheel_Materials_Num = 1;
        ciw.Wheel_Material = _IlderWheelMaterial;
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
        bb.Part_Material = _barrelMaterial;
        bb.Create();

        var ct = _vehicle.GetComponentInChildren<Turret_Base_CS>();
        ct.Part_Mesh = _turretMesh;
        ct.Materials[0] = _turretMaterial;
        ct.Materials_Num = 1;
        ct.Offset_X = _turretOffset.x;
        ct.Offset_Y = _turretOffset.y;
        ct.Offset_Z = _turretOffset.z;
        ct.Create();

        var cb = _vehicle.GetComponentInChildren<Cannon_Base_CS>();
        cb.Part_Mesh = _cannonMesh;
        cb.Part_Material = _cannonMaterial;
        cb.Create();

        var mbs = _vehicle.GetComponentInChildren<MainBody_Setting_CS>();
        mbs.Body_Mesh = _mainBodyMesh;
        mbs.Materials = _mainBodyMaterial;
        mbs.Materials_Num = _mainBodyMaterial.Length;
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
