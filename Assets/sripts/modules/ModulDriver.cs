using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulDriver : ModulBase
{
   private SkinnedMeshRenderer meshRenderer;
   private MeshCollider collider;
    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        collider = gameObject.GetComponent<MeshCollider>();
        UpdateCollider();
    }
    private void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }
    public override void GetDamage(int damage)
    {
        print("driver damaged");
        base.GetDamage(damage);
        switch (hp)
        {

            case 0:
                print("Modul destroed");
                rearWheelDrive.enabled = false;
                break;

            case int n when (n <= 3):
                if (isModelDamaged)
                {
                    return;
                }
                isModelDamaged = true;
                print("Modul damaged");

                rearWheelDrive.rotationSpeed /=2;
                break;


        }
    }
}
