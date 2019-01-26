using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DickController : MonoBehaviour {

    [SerializeField]
    private PathTool wayPoints;
    [SerializeField]
    int wayPointCount = 0;
    private Transform MovetoPos;
    bool move = false;

    void Start() {
        transform.position = wayPoints.nodes[0].position;
        onMoveTo();

    }

    void Update() {

    }
    public void onMoveTo() {
        StartCoroutine(wait());
    }

    IEnumerator wait() {
        yield return new WaitForSecondsRealtime(1f);
        move = true;
        wayPointCount++;
        if (wayPointCount >= wayPoints.nodes.Count) wayPointCount = 0;
        MovetoPos = wayPoints.nodes[wayPointCount];
        this.GetComponent<DoTweenPositionHelper>().onTweenTo(MovetoPos);
    }
}