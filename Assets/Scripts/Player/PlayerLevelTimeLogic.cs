using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelTimeLogic : MonoBehaviour {

    [SerializeField]
    public float LevelTime;
    [SerializeField]
    private float StartTime;
    [SerializeField]
    private float tempTime = 0f;
    [SerializeField]
    private Vector3 startpos;
    public bool timer = false;
    [SerializeField]
    public Text Timetext;
    public bool updateTime = false;

	// Use this for initialization

    public void init() {
        LevelTime = 0.0f;
        startpos = transform.position;
        StartTime = Time.timeSinceLevelLoad;
        timer = true;
        Timetext.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position != startpos && timer) {
            timer = false;
            tempTime = Time.timeSinceLevelLoad;
            updateTime = true;
        }

        LevelTime =Time.timeSinceLevelLoad -tempTime;

        if(updateTime)
            Timetext.text = "" + System.Math.Round(LevelTime, 2); ;
	}

}
