using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    
    public GameObject explosion; //Explosion Prefab

    [HideInInspector]
    public int damage; //Transferred from Hydra AI to the Hitbox. User doesn't need to see this.
    
    private void OnTriggerEnter(Collider other)
    {
        //If the Fireball collides with any object that isn't a Projectile, Skybox, gameobject named Colliders, Trigger, or Hydra
        if (other.gameObject.tag != "Projectile" && other.gameObject.tag != "Skybox" && other.gameObject.name != "Colliders" && other.GetComponent<Collider>().isTrigger == false)// && other.transform.parent.GetComponent<Hydra_AI>() == false)
        {
            GameObject explo = Instantiate(explosion, transform.position, transform.rotation); //Spawn explosion
            explo.GetComponent<Hitbox>().damage = damage; //Transfer the damage from Hydra AI to Hitbox
            Destroy(gameObject); //Destroy this object
        }
    }
}
