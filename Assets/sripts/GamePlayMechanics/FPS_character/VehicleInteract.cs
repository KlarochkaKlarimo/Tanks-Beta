using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteract : Interactable
{
    [SerializeField] private MonoBehaviour[] monoBehaviours;

    private void Awake()
    {
        foreach (var monoBehaviour in monoBehaviours)
        {
            monoBehaviour.enabled = false;
        }
    }

    public override void Interact(GameObject obj)
    {
        foreach (var monoBehaviour in monoBehaviours)
        {
            monoBehaviour.enabled = true;
        }
        obj.SetActive(false);
    }
}
