using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelObjectController : MonoBehaviour {

    [SerializeField]
    private PathTool wayPoints;
    [SerializeField]
    int wayPointCount = 0;
    [SerializeField]
    private bool pingpong = false;

    public bool AutoRotate = false;
    [SerializeField]
    private Vector3 RotationVector;
    [SerializeField]
    private float rotationSpeed;
    bool reverse = false;
    private Transform MovetoPos;
    bool move = false;

    void Start() {
        if (AutoRotate) this.transform.DORotate(RotationVector, rotationSpeed).SetLoops(-1, LoopType.Incremental);
        transform.position = wayPoints.nodes[0].position;
        onMoveTo();

    }

    public void onMoveTo() {
        move = true;
        if (pingpong && reverse) wayPointCount--;
        if (pingpong && !reverse) wayPointCount++;
        if (!pingpong) wayPointCount++;
        
        if (wayPointCount >= wayPoints.nodes.Count && !pingpong) wayPointCount = 0;
        if (wayPointCount == 0 && pingpong && reverse == true) { reverse = false; }
        if (wayPointCount >= wayPoints.nodes.Count && pingpong && reverse == false) { reverse = true; wayPointCount = wayPointCount - 2; }
        MovetoPos = wayPoints.nodes[wayPointCount];
        this.GetComponent<DoTweenHelper>().onTweenTo(MovetoPos);
    }
}