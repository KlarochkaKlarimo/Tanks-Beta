using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Props", menuName = "ScriptableObjects/Props")]

public class PropCollections : ScriptableObject
{
    public List<Prop> props;  
}

[Serializable] 
public class Prop
{
    public Prop(GameObject prefabObject, PropType propType)
    {
        prefab = prefabObject;
        type = propType;
    }
    public GameObject prefab;
    public PropType type;
}

public enum PropType
{
    building,
    tree,
    bush
}