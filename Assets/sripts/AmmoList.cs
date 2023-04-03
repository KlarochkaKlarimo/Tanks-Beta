using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AmmoList
{
    public GameObject prefab;
    public string ammoName;

	public int bulletPenetration;

	public float reloadTime = 3f;
	public int velocity = 250;
	public int recoilForce = 50000;

	public GameObject explosionPrefab;
	public float explosionForce = 20000f;
	public float explosionRadius = 5f;
	public int lifeTimeOfTheProjectile = 5;

	public AudioClip fireSoundClip;
	public GameObject groundSmoke;
	public GameObject fireSmoke;
}
