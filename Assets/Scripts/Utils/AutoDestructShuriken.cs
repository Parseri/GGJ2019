using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestructShuriken : MonoBehaviour {

    [SerializeField]
    ParticleSystem pSystem;
    public bool OnlyDeactivate;

    void OnEnable() {
        StartCoroutine("CheckIfAlive");
    }

    IEnumerator CheckIfAlive() {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            if (!pSystem.IsAlive(true)) {
                if (OnlyDeactivate)  GameObject.Destroy(this.gameObject);
            }
        }
    }
}
