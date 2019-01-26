using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEvaluator {
    public enum InputButton {
        UNDEFINED,
        LEFT,
        RIGHT,
        SUICIDE,
        JUMP
    }

    public enum InputEvent {
        UNDEFINED,
        UP,
        DOWN
    }

    public class InputSaver {
        public InputEvent ie;
        public InputButton ib;
        public float ts;
        public Vector3 pos;
        public Quaternion rot;
        public bool hasTrans;

        public InputSaver(InputButton bu, InputEvent ev, float stamp) {
            ie = ev;
            ib = bu;
            ts = stamp;
            hasTrans = false;
        }

        public InputSaver(InputButton bu, InputEvent ev, float stamp, Vector3 p, Quaternion r) {
            ie = ev;
            ib = bu;
            ts = stamp;
            pos = p;
            rot = r;
            hasTrans = true;
        }
    }
    private List<InputSaver> inputs;
    public List<InputSaver> Inputs {
        get { return inputs; }
        set { inputs = value; }
    }
    private int replayIndex = -1;
    private float replayStartTs;
    private bool allFinished = false;
    private float originalStartTs = -1;
    public bool Replaying { get { return replayIndex >= 0 && !allFinished; } }
    public bool Ended { get { return allFinished; } }
    public bool started = false;
    public bool player = false;

    public GameObject instantiatedObject;
    private PlayerController2D controller;


    public void DestroyOldObject() {
        GameObject.Destroy(instantiatedObject);
    }

    public void SetController(GameObject prefab) {
        GameObject.Destroy(instantiatedObject);
        instantiatedObject = GameObject.Instantiate(prefab);
        controller = instantiatedObject.GetComponent<PlayerController2D>();
        instantiatedObject.layer = LayerMask.NameToLayer("Default");
        allFinished = false;
        replayIndex = -1;
        started = false;
    }

    public InputEvaluator(InputEvaluator other) {
        inputs = other.Inputs;
        Debug.Log("frames: " + inputs.Count);
        player = false;
    }

    public InputEvaluator() {
        inputs = new List<InputSaver>();
        player = true;
    }

    public void StartReplay() {
        replayIndex = 0;
        replayStartTs = Time.time;
        allFinished = false;
        started = true;
    }

    public bool IsDead() {
        return instantiatedObject.GetComponent<playerDeathLogic>().dying;
    }

    public void StartRecording() {
        originalStartTs = -1;
        replayIndex = -1;
        inputs.Clear();
        allFinished = false;
        originalStartTs = Time.time;
        started = true;
    }

    public void EvaluateInput(InputButton button, InputEvent ievent) {
        if (!player) {
            if (!started) StartReplay();
            if (replayIndex >= 0) {
                if (replayIndex < inputs.Count) {
                    for (int i = replayIndex; i < inputs.Count; ++i) {
                        InputSaver saver = inputs[replayIndex];
                        var ts = Time.time - replayStartTs;
                        if (ts >= saver.ts) {
                            button = saver.ib;
                            ievent = saver.ie;
                            replayIndex++;
                            if (saver.hasTrans) {
                                instantiatedObject.transform.position = Vector3.Lerp(instantiatedObject.transform.position, saver.pos, 0.1f);
                                instantiatedObject.transform.rotation = Quaternion.Lerp(instantiatedObject.transform.rotation, saver.rot, 0.1f);
                            }
                            SendMovementToController(button, ievent);
                        } else
                            return;
                    }
                } else {
                    allFinished = true;
                    ResetMovement();
                    return;
                }
            }
        } else {

            var ts = Time.time - originalStartTs;
            if (IsDead()) {
                ResetMovement();
                return;
            }
            Debug.Log("adding more inputs: " + button.ToString() + ", event: " + ievent + ", ts: " + ts);
            Transform trans = instantiatedObject.transform;
            if (controller.ShouldSaveTransform())
                inputs.Add(new InputSaver(button, ievent, ts, trans.position, trans.rotation));
            else
                inputs.Add(new InputSaver(button, ievent, ts));
            SendMovementToController(button, ievent);
        }
    }

    public void ResetMovement() {
        controller.StopMovement();
    }

    private void SendMovementToController(InputButton button, InputEvent ievent) {
        if (button == InputButton.SUICIDE) {
            if (player) {
                GameObject parent = GameObject.FindGameObjectWithTag("SplatParent");
                instantiatedObject.GetComponent<PlayerSplatterLogic>().SpawnChunkParticles(instantiatedObject.transform.position, Vector3.up, parent.transform);
                instantiatedObject.GetComponent<playerDeathLogic>().die();
            }
        }
        if (button == InputButton.JUMP)
            controller.Jump(ievent == InputEvent.DOWN);
        if (button == InputButton.RIGHT && ievent == InputEvent.DOWN)
            controller.MoveRight();
        if (button == InputButton.LEFT && ievent == InputEvent.DOWN)
            controller.MoveLeft();
        if (ievent == InputEvent.DOWN && button == InputButton.RIGHT && controller.MovingLeft())
            controller.Jump(true);
        if (ievent == InputEvent.DOWN && button == InputButton.LEFT && controller.MovingRight())
            controller.Jump(true);
    }
}
