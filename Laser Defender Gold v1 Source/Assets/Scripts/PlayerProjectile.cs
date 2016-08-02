using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class PlayerProjectile : MonoBehaviour {

    public float LaserPower;
    public GameObject ShieldHitExplosion;

    void Awake()
    {
        LaserPower = GameManager.instance.LaserPower;
    }

    public void HitShield()
    {
        Instantiate(ShieldHitExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //Destroy Projectile when it leaves the screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
