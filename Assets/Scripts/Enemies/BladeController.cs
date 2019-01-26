using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DoTweenPositionHelper))]
public class BladeController : MonoBehaviour {
    [SerializeField]
    float BladeMoveSpeed = 15f;
    [SerializeField]
    float BladeRotateSpeed = 15f;
    [SerializeField]
    private PathTool wayPoints;
    [SerializeField]
    int wayPointCount = 0;
    private Transform MovetoPos;
    bool move = false;


    // Use this for initialization
    void Start() {
        transform.position = wayPoints.nodes[0].position;
        onMoveTo();

    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(-Vector3.forward * (Time.fixedDeltaTime * BladeRotateSpeed), Space.World);
    }


    public void onMoveTo() {
        move = true;
        wayPointCount++;
        if (wayPointCount >= wayPoints.nodes.Count) wayPointCount = 0;
        MovetoPos = wayPoints.nodes[wayPointCount];
        this.GetComponent<DoTweenPositionHelper>().onTweenTo(MovetoPos);
    }
}
