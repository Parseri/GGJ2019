using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject start = GameObject.FindGameObjectWithTag("Start");
        this.transform.position = start.transform.position +  new Vector3(0,1,0);
        StartCoroutine(wait());
	}


    public void onReset() {
        GameObject start = GameObject.FindGameObjectWithTag("Start");
        this.transform.position = start.transform.position + new Vector3(0, 1, 0);
        StartCoroutine(wait());
    }

    IEnumerator wait() {
        yield return new WaitForSecondsRealtime(0.1f);
        this.GetComponent<PlayerLevelTimeLogic>().init();
        this.GetComponent<PlayerAutoController2D>().enabled = true;
        this.GetComponent<PlayerAutoController2D>().PlayerMovement = 1;
        this.GetComponent<playerDeathLogic>().dying = false;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.GetComponent<PlatformerMotor2D>().enabled = true;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

}
