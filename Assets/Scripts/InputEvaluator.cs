using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEvaluator {
    public enum InputButton {
        UNDEFINED,
        LEFT,
        RIGHT,
        SUICIDE
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

        public InputSaver(InputButton bu, InputEvent ev, float stamp) {
            ie = ev;
            ib = bu;
            ts = stamp;
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

    public void SetController(GameObject prefab) {
        GameObject.Destroy(instantiatedObject);
        instantiatedObject = GameObject.Instantiate(prefab);
        controller = instantiatedObject.GetComponent<PlayerController2D>();
        allFinished = false;
        replayIndex = -1;
        started = false;
    }

    public InputEvaluator(InputEvaluator other) {
        inputs = other.Inputs;
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
                    InputSaver saver = inputs[replayIndex];
                    var ts = Time.time - replayStartTs;
                    if (ts >= saver.ts) {
                        button = saver.ib;
                        ievent = saver.ie;
                        replayIndex++;
                        SendMovementToController(button, ievent);
                    } else
                        return;
                } else {
                    allFinished = true;
                    return;
                }
            }
        } else {

            var ts = Time.time - originalStartTs;
            if (IsDead()) {
                button = InputButton.UNDEFINED;
                ievent = InputEvent.UP;
            }
            inputs.Add(new InputSaver(button, ievent, ts));
            SendMovementToController(button, ievent);
        }
    }

    private void SendMovementToController(InputButton button, InputEvent ievent) {
        controller.MoveLeft(false);
        if (button == InputButton.RIGHT && ievent == InputEvent.DOWN)
            controller.MoveRight(true);
        if (button == InputButton.LEFT && ievent == InputEvent.DOWN)
            controller.MoveLeft(true);
        if (ievent == InputEvent.DOWN && button == InputButton.RIGHT && controller.MovingLeft())
            controller.Jump();
        if (ievent == InputEvent.DOWN && button == InputButton.LEFT && controller.MovingRight())
            controller.Jump();
    }
}
