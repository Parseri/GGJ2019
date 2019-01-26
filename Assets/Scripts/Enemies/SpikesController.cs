using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DoTweenPositionHelper))]

public class SpikesController : MonoBehaviour {
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

    public void onMoveTo() {
        move = true;
        wayPointCount++;
        if (wayPointCount >= wayPoints.nodes.Count) wayPointCount = 0;
        MovetoPos = wayPoints.nodes[wayPointCount];
        this.GetComponent<DoTweenPositionHelper>().onTweenTo(MovetoPos);
    }
}
