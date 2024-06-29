using UnityEngine;
public class ObjectToFind : MonoBehaviour
{
    public bool isLeft;
    public int trackNumber;
    public ObjectToFind wheel;
    [SerializeField] private ObjectType type;
    
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
    wheel,
    supportWheel
}