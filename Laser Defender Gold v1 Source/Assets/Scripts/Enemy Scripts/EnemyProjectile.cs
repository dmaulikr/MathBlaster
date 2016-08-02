using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
	
	public float damage = 100f;
    public GameObject deathParticle;

	public float GetDamage(){
		return damage;
	}
	
	public void Hit() {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
	}
	
    //Destroy Projectile when it leaves the screen
	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
