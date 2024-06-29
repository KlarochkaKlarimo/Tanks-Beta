using UnityEngine;
public class ObjectToFind : MonoBehaviour
{
    [SerializeField] private ObjectType type;
    public ObjectToFind wheel;
    private void Awake()
    {
        Destroy(this);
    }
    public ObjectType GetType() { return type; }
    
    public ObjectResources GetResources(){return new ObjectResources(gameObject, type);}
}
public class ObjectResources
{
    public Mesh _mesh;
    public Material[] _materials;
    public ObjectType type;

    public ObjectResources(GameObject resourceObject, ObjectType _type)
    {
        try 
        {
            _mesh = resourceObject.GetComponent<MeshFilter>().mesh;
            _materials = resourceObject.GetComponent<MeshRenderer>().materials;
            type = _type;
        }
        catch 
        {
            Debug.Log(resourceObject.name);
        }
    }
}
public enum ObjectType
{
    body,
    turret,
    cannon,
    barrel,
    track,
    spoketWheel,
    idlerWheel,
    suspentionR,
    suspentionL,
    Wheel
}