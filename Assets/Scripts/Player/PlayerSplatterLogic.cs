using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSplatterLogic : MonoBehaviour {

    [Header("Particles")]
    public Splatter _splatter;
    public Splatter DynamicSplatter;
    public ParticleSystem chunkParticles;
    public LayerMask CollisionLayer;

    public void Splatt( Vector3 Position, Collider2D other) {

        if (other.tag == "Enemy") {
            Debug.Log("ENEMY");
            Splatter splat = Instantiate(DynamicSplatter, Position, Quaternion.identity);
            float scale = Random.Range(2f, 3f);
            splat.transform.localScale = new Vector3(scale, scale, scale);
            splat.transform.SetParent(other.gameObject.transform, true);
            splat.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }

        RaycastHit2D hit = Physics2D.CircleCast(Position, 3, Vector2.up, Mathf.Infinity, CollisionLayer);
        if (hit) {
            int k = Random.Range(2, 4);
            for (int i = 0; i < k; i++) {
                Instantiate(_splatter, Position + new Vector3(Random.Range(0, 1), 0, 0), Quaternion.identity);
            }
        }
    }

    public void SpawnChunkParticles(Vector3 Position, Vector3 Normal, Transform Parent) {
        ParticleSystem p = Instantiate(chunkParticles, Position, Quaternion.LookRotation(Normal), Parent);
        p.name = chunkParticles.name;
        p.Play();

    }
}
