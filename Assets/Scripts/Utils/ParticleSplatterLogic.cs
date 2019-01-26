using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSplatterLogic : MonoBehaviour {

    public ParticleSystem part;
    public Splatter _splatter;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
       
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        Vector3 lastPos = Vector3.zero;
        for (int i = 0; i < numCollisionEvents; i++) {
            if (i == 1) break;
            if (Random.Range(1, 10) > 5) break;
            Vector3 pos = collisionEvents[i].intersection;
            if (pos != lastPos) {
                Splatter splat = Instantiate(_splatter, pos, Quaternion.identity);
                float scale = Random.Range(1.5f, 3f);
                splat.transform.localScale = new Vector3(scale, scale, scale);
                lastPos = pos;
               
            }
        }
     }
}
