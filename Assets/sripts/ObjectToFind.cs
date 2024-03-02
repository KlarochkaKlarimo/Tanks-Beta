using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ObjectToFind : MonoBehaviour
{
    [SerializeField] private ObjectType type;
    private void Awake()
    {
        Destroy(this);
    }
    public ObjectResources GetResources(){return new ObjectResources(gameObject, type);}
}
public class ObjectResources
{
    public Mesh _mesh;
    public Material[] _materials;
    public ObjectType type;
    public ObjectResources(GameObject resourceObject, ObjectType _type)
    {
        _mesh = resourceObject.GetComponent<MeshFilter>().mesh;
        _materials = resourceObject.GetComponent<MeshRenderer>().materials;
        type = _type;
    }
}
public enum ObjectType
{
    body,
    tower
}